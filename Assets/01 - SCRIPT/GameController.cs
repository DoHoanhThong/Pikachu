using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Unity.Burst.Intrinsics.X86.Avx;

public class GameController : Singleton<GameController>
{
    
    [SerializeField] bool isSpecial;
    public int countClick;
    public GridCell cell1, cell2;
    public GridLayoutGroup gridLayoutGroup;
    public int[,] grid; // Mảng lưu giá trị của mỗi ô trong lưới
    public LineRenderer lineRenderer;
    [SerializeField]private List<GridCell> path;
    int rows, cols;
    public List<GameObject> blocks = new List<GameObject>();
    [SerializeField] Image cd;
    int time, begin = 60;
    List<int> used;
    // Start is called before the first frame update
    void Start()
    {
        countClick = 0;
        used = new List<int>();
        time = begin;
        StartCoroutine(CountDown());
        RandomSprite();
        InitializeGrid();
    }
    public void InitializeGrid()
    {
        rows = 11;
        cols = 22;
        
    }
    IEnumerator CountDown()
    {
        while (time > 0)
        {
            time -= 1;
            cd.fillAmount -= (float)1 / begin;
            yield return new WaitForSeconds(1);
        }
    }
    void RandomSprite()
    {
        int dem = 0;
        while (dem < blocks.Count / 2)
        {
            int index = Random.Range(1, 37);
            if (dem >= 36)
            {
                used.Add(index);
                dem += 1;
            }
            else if (dem < 36 && (!used.Contains(index) || used.Count == 0))
            {
                used.Add(index);
                dem += 1;
            }
        }
        RandomBlocks();
    }
    void RandomBlocks()
    {
        List<int> assigned = new List<int>();
        int count = 0;
        while (assigned.Count < blocks.Count - 1)
        {
            Sprite randomSprite = Resources.Load<Sprite>("Blocks/" + used[count]);
            int index = Random.Range(0, blocks.Count);

            //1
            if (assigned.Count == 0)
            {
                assigned.Add(index);
            }
            else
            {
                while (assigned.Count == 0 || assigned.Contains(index))
                {
                    index = Random.Range(0, blocks.Count);
                }
                assigned.Add(index);
            }

            //2
            int index2 = Random.Range(0, blocks.Count);
            while (assigned.Contains(index2))
            {
                index2 = Random.Range(0, blocks.Count);
            }
            assigned.Add(index2);
            blocks[index].transform.GetChild(0).GetComponent<Image>().sprite = randomSprite;
            blocks[index].transform.GetComponent<GridCell>().id = used[count];

            blocks[index2].transform.GetComponent<GridCell>().id = used[count];
            blocks[index2].transform.GetChild(0).GetComponent<Image>().sprite = randomSprite;
            count++;
        }

    }

    public void CheckAndDrawPath()
    {
        // Kiểm tra xem giá trị của hai ô có giống nhau không
        if (cell1.id != cell2.id)
        {
            cell1 = null;
            cell2 = null;
            countClick = 0;
            Debug.LogError("Không kết nối");
            return; // Giá trị không giống nhau, không kết nối
        }
        CheckSpecial();
        if (isSpecial)
        {
            Debug.LogError("isSpecial");
            isSpecial = false;
            CorrectChoice();
            return;
        }
        // Nếu giá trị giống nhau, kiểm tra kết nối giữa hai ô
        path = FindPath(cell1, cell2);

        if (path != null && path.Count>0)
        {
            CorrectChoice();
            DrawPath();
            
        }
        else
        {
            WrongChoice();
            countClick = 0;
            
        }
        cell1 = null;
        cell2 = null;
    }
    void CheckSpecial()
    {
        foreach(GridCell nb in GetNeighbors(cell1))
        {
            if(nb.col==cell2.col && nb.row == cell2.row)
            {
                lineRenderer.positionCount = 0;
                lineRenderer.positionCount = 2;
                lineRenderer.SetPosition(0, (Vector2)cell1.transform.position);
                lineRenderer.SetPosition(1, (Vector2)cell2.transform.position);
                isSpecial = true; break;
            }
        }
    }
    private List<GridCell> FindPath(GridCell start, GridCell end)
    {
        Queue<GridCell> queue = new Queue<GridCell>();
        Dictionary<GridCell, GridCell> cameFrom = new Dictionary<GridCell, GridCell>();
        List<GridCell> path = new List<GridCell>();

        queue.Enqueue(start);
        cameFrom[start] = null;
        while (queue.Count > 0)
        {
            GridCell current = queue.Dequeue();

            if (current.row == end.row && current.col == end.col)
            {
                Debug.LogError("hihiocho!");
                // Truy vết ngược để lấy đường đi
                while (current != null)
                {

                    path.Add(current);
                    current = cameFrom[current];
                }

                path.Reverse();
                //return path;
            }
            
            foreach (GridCell neighbor in GetNeighbors(current))
            {
                if (!cameFrom.ContainsKey(neighbor) && !neighbor.isAcitve || (neighbor.col==end.col && neighbor.row==end.row))
                {
                    Debug.LogError("duong di: " + neighbor.row + " " + neighbor.col);
                    queue.Enqueue(neighbor);
                    cameFrom[neighbor] = current;
                }
            }
        }

        //return null; // Không tìm thấy đường đi
        return CheckPath(path);
    }
    private List<GridCell> CheckPath(List<GridCell> path)
    {
        int dem = 0;
        if (path.Count > 2)
        {
            for (int i = 1; i < path.Count - 1; i++)
            {
                GridCell current = path[i];
                GridCell previous = path[i - 1];
                GridCell next = path[i + 1];

                // Kiểm tra xem có gấp khúc quá 2 lần hay không
                if (!AreCellsInLine(previous, current, next))
                {
                    dem++;
                    Debug.LogError("+1 gap khuc");
                    if (dem > 2)
                    {
                        Debug.LogError("gap khuc qua 2 lan!");
                        // Gấp khúc quá 2 lần, return null hoặc làm gì đó tương ứng
                        return null;
                    }
                }
            }
            
        }
        return path;
    }

    // Hàm kiểm tra xem ba ô có nằm trên cùng một đường thẳng hay không
    bool AreCellsInLine(GridCell c1, GridCell c2, GridCell c3)
    {
        return (c1.row == c2.row && c2.row == c3.row) || (c1.col == c2.col && c2.col == c3.col);
    }
    private List<GridCell> GetNeighbors(GridCell cell)
    {
        List<GridCell> neighbors = new List<GridCell>();

        // Kiểm tra ô phía trên
        if (cell != null)
        {
            int up = (cell.row - 1) * cols + cell.col;
            int down = (cell.row + 1) * cols + cell.col;
            int left = (cell.row) * cols + (cell.col - 1);
            int right = (cell.row) * cols + cell.col + 1;
            if (cell.row >= 1)
            {
                GridCell topNeighbor = gridLayoutGroup.transform.GetChild(up).GetComponent<GridCell>();

                neighbors.Add(topNeighbor);
            }

            // Kiểm tra ô phía dưới
            if (cell.row <= 9)
            {

                GridCell bottomNeighbor = gridLayoutGroup.transform.GetChild(down).GetComponent<GridCell>();

                neighbors.Add(bottomNeighbor);
            }

            // Kiểm tra ô bên trái
            if (cell.col >= 1)
            {

                GridCell leftNeighbor = gridLayoutGroup.transform.GetChild(left).GetComponent<GridCell>();

                neighbors.Add(leftNeighbor);
            }

            // Kiểm tra ô bên phải
            if (cell.col <= 20)
            {

                GridCell rightNeighbor = gridLayoutGroup.transform.GetChild(right).GetComponent<GridCell>();

                neighbors.Add(rightNeighbor);
            }
        }
        
        return neighbors;
    }

    // Hàm vẽ đường đi sử dụng Line Renderer
    private void DrawPath()
    {
        if (lineRenderer != null && path != null && path.Count > 1)
        {
            lineRenderer.positionCount = 0;
            lineRenderer.positionCount = path.Count;
            for (int i = 0; i < path.Count; i++)
            {
                Vector3 worldPosition = new Vector3(path[i].transform.position.x, path[i].transform.position.y, 0);
                lineRenderer.SetPosition(i, worldPosition);
            }
        }
    }
    void DrawSpecial(List<Vector2> pos)
    {
        if (pos.Count > 0)
        {
            lineRenderer.positionCount = 0;
            lineRenderer.positionCount = pos.Count;
            for (int i = 0; i < pos.Count; i++)
            {
                Debug.Log(pos[i]);
                lineRenderer.SetPosition(i, pos[i]);
            }
        }
    }
    void CorrectChoice()
    {
        
        //PlaySound.instance.PlayCorrectSound();
        
        cell1.Correct();
        cell2.Correct();
        cell2 = null;
        cell1 = null;
        Debug.LogError("Có kết nối");
        StartCoroutine(timeDrawCD());
        
    }
    void WrongChoice()
    {
        cell1.Wrong();
        cell2.Wrong();
        cell1 = null;
        cell2 = null;
        //PlaySound.instance.PlayWrongSound();
        Debug.LogError("Không kết nối");
    }
    IEnumerator timeDrawCD()
    {
        yield return new WaitForSeconds(0.5f);
        lineRenderer.positionCount = 0;
        path.Clear();
    }
}

