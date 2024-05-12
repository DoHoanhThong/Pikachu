using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FindPath : MonoBehaviour
{
    [SerializeField] GridLayoutGroup gridLayoutGroup;
    int cols = 22;
    int rows = 11;

    public List<GridCell> FindPaths(GridCell c1, GridCell c2)
    {
        if ((c1.col == c2.col) || (c1.row == c2.row))
        {
            c2.isAcitve = false;
            c1.isAcitve = false;
            if (checkVertical(c1.row, c2.row, c1.col) || checkHorizontal(c1.col, c2.col, c1.row))
            {
                return new List<GridCell> { c1, c2 };
            }
            c1.isAcitve = true;
            c2.isAcitve = true;
        }
        if (checkRectRow(c1, c2) != null) return checkRectRow(c1, c2);
        else if (checkRectCol(c1, c2) != null) return checkRectCol(c1, c2);

        if (checkMoreLineY(c1, c2, 1) != null && checkMoreLineY(c2, c1, -1) == null) return checkMoreLineY(c1, c2, 1);
        else if (checkMoreLineY(c1, c2, -1) != null && checkMoreLineY(c2, c1, 1) == null) return checkMoreLineY(c1, c2, -1);
        else if (checkMoreLineY(c1, c2, -1) != null && checkMoreLineY(c2, c1, 1) != null)
            return (checkMoreLineY(c1, c2, -1).Count < checkMoreLineY(c1, c2, 1).Count) ? checkMoreLineY(c1, c2, -1) : checkMoreLineY(c1, c2, 1);

        if (checkMoreLineX(c1, c2, 1) != null && checkMoreLineX(c2, c1, -1) == null) return checkMoreLineX(c1, c2, 1);
        else if (checkMoreLineX(c1, c2, -1) != null && checkMoreLineX(c2, c1, 1) == null) return checkMoreLineX(c1, c2, -1);
        else if (checkMoreLineX(c1, c2, -1) != null && checkMoreLineX(c2, c1, 1) != null)
            return (checkMoreLineX(c1, c2, -1).Count < checkMoreLineX(c1, c2, 1).Count) ? checkMoreLineX(c1, c2, -1) : checkMoreLineX(c1, c2, 1);
        return null;

    }
    //check hang, dieu kien: cung` cot.
    private bool checkVertical(int row1, int row2, int col)
    {
        int min = Mathf.Min(row1, row2);
        int max = Mathf.Max(row1, row2);
        if (min == max) return false;
        for (int y = min; y <= max; y++)
        {
            if (gridLayoutGroup.transform.GetChild(y * cols + col).GetComponent<GridCell>().isAcitve)
            {
                return false;
            }
        }
        return true;
    }
    //check cot, dieu kien : cung` hang`
    private bool checkHorizontal(int col1, int col2, int row)
    {
        int min = Mathf.Min(col1, col2);
        int max = Mathf.Max(col1, col2);
        if (min == max) return false;
        for (int y = min; y <= max; y++)
        {
            if (gridLayoutGroup.transform.GetChild(row * cols + y).GetComponent<GridCell>().isAcitve)
            {
                return false;
            }
        }
        return true;
    }
    //check hình Z ngang
    private List<GridCell> checkRectRow(GridCell c1, GridCell c2)
    {
        if (c1.row == c2.row || c1.col == c2.col) return null;
        GridCell cMinY = c1, cMaxY = c2;
        if (c1.row > c2.row)
        {
            cMinY = c2;
            cMaxY = c1;
        }
        List<GridCell> result = new List<GridCell>();
        result.Add(cMinY);
        cMaxY.isAcitve = false;
        cMinY.isAcitve = false;
        for (int y = cMinY.row + 1; y < cMaxY.row; y++)
        {
            if (checkVertical(cMinY.row, y, cMinY.col) && checkHorizontal(cMinY.col, cMaxY.col, y) && checkVertical(y, cMaxY.row, cMaxY.col))
            {
                GridCell tmpc1 = gridLayoutGroup.transform.GetChild(y * cols + cMinY.col).GetComponent<GridCell>();
                GridCell tmpc2 = gridLayoutGroup.transform.GetChild(y * cols + cMaxY.col).GetComponent<GridCell>();
                result.Add(tmpc1); result.Add(tmpc2);
                result.Add(cMaxY);
                return result;
            }
        }
        if (checkVertical(cMinY.row, cMaxY.row, cMinY.col) && checkHorizontal(cMinY.col, cMaxY.col, cMaxY.row))
        {
            GridCell tmpc3 = gridLayoutGroup.transform.GetChild(cMaxY.row * cols + cMinY.col).GetComponent<GridCell>();
            result.Add(tmpc3);
            result.Add(cMaxY);
            return result;
        }
        else if (checkVertical(cMinY.row, cMaxY.row, cMaxY.col) && checkHorizontal(cMinY.col, cMaxY.col, cMinY.row))
        {
            GridCell tmpc4 = gridLayoutGroup.transform.GetChild(cMinY.row * cols + cMaxY.col).GetComponent<GridCell>();
            result.Add(tmpc4);
            result.Add(cMaxY);
            return result;
        }
        cMinY.isAcitve = true;
        cMaxY.isAcitve = true;
        return null;
    }
    //check hình Z đứng
    private List<GridCell> checkRectCol(GridCell c1, GridCell c2)
    {
        if (c1.col == c2.col || c1.row == c2.row) return null;
        GridCell cMinX = c1, cMaxX = c2;
        if (c1.col > c2.col)
        {
            cMinX = c2;
            cMaxX = c1;
        }
        List<GridCell> result = new List<GridCell>();
        result.Add(cMinX);
        cMaxX.isAcitve = false;
        cMinX.isAcitve = false;
        for (int x = cMinX.col + 1; x < cMaxX.col; x++)
        {
            if (checkHorizontal(cMinX.col, x, cMinX.row) && checkVertical(cMinX.row, cMaxX.row, x) && checkHorizontal(x, cMaxX.col, cMaxX.row))
            {
                GridCell tmpc1 = gridLayoutGroup.transform.GetChild(cMinX.row * cols + x).GetComponent<GridCell>();
                GridCell tmpc2 = gridLayoutGroup.transform.GetChild(cMaxX.row * cols + x).GetComponent<GridCell>();
                result.Add(tmpc1); result.Add(tmpc2);
                result.Add(cMaxX);
                return result;
            }
        }
        cMaxX.isAcitve = true;
        cMinX.isAcitve = true;
        return null;
    }
    //mo rong theo hang
    private List<GridCell> checkMoreLineY(GridCell c1, GridCell c2, int type)
    {
        if (c1.col == c2.col) return null;
        GridCell cMinY = c1, cMaxY = c2;
        if (c1.row > c2.row)
        {
            cMinY = c2;
            cMaxY = c1;
        }
        int rowStart = cMaxY.row + type;
        if (type == -1)
        {
            rowStart = cMinY.row + type;//0
        }
        cMinY.isAcitve = false;
        cMaxY.isAcitve = false;
        while (rowStart >= 0 && rowStart <= rows - 1)
        {
            if (gridLayoutGroup.transform.GetChild(rowStart * cols + cMinY.col).GetComponent<GridCell>().isAcitve
                || gridLayoutGroup.transform.GetChild(rowStart * cols + cMaxY.col).GetComponent<GridCell>().isAcitve)
            {
                break;
            }
            if (checkHorizontal(cMinY.col, cMaxY.col, rowStart) && checkVertical(rowStart, cMaxY.row, cMaxY.col) && checkVertical(cMinY.row, rowStart, cMinY.col))
            {
                List<GridCell> result = new List<GridCell>();
                result.Add(cMinY);
                GridCell tmpc1 = gridLayoutGroup.transform.GetChild(rowStart * cols + cMinY.col).GetComponent<GridCell>();
                GridCell tmpc2 = gridLayoutGroup.transform.GetChild(rowStart * cols + cMaxY.col).GetComponent<GridCell>();
                result.Add(tmpc1); result.Add(tmpc2);
                result.Add(cMaxY);
                return result;
            }
            rowStart += type;
        }
        cMinY.isAcitve = true;
        cMaxY.isAcitve = true;
        return null;
    }
    //mo rong theo cot
    private List<GridCell> checkMoreLineX(GridCell c1, GridCell c2, int type)
    {
        if (c1.row == c2.row) return null;

        GridCell cMinX = c1, cMaxX = c2;
        if (c1.col > c2.col)
        {
            cMinX = c2;
            cMaxX = c1;
        }

        int colStart = cMaxX.col + type;
        if (type == -1)
        {
            colStart = cMinX.col + type;

        }
        cMinX.isAcitve = false;
        cMaxX.isAcitve = false;
        while (colStart >= 0 && colStart <= cols - 1)
        {
            if (gridLayoutGroup.transform.GetChild(cMinX.row * cols + colStart).GetComponent<GridCell>().isAcitve
                || gridLayoutGroup.transform.GetChild(cMaxX.row * cols + colStart).GetComponent<GridCell>().isAcitve)
            {
                break;
            }
            if (checkVertical(cMinX.row, cMaxX.row, colStart) && checkHorizontal(cMinX.col, colStart, cMinX.row) && checkHorizontal(cMaxX.col, colStart, cMaxX.row))
            {
                List<GridCell> result = new List<GridCell>();
                result.Add(cMinX);
                GridCell tmpc1 = gridLayoutGroup.transform.GetChild(cMinX.row * cols + colStart).GetComponent<GridCell>();
                GridCell tmpc2 = gridLayoutGroup.transform.GetChild(cMaxX.row * cols + colStart).GetComponent<GridCell>();
                result.Add(tmpc1); result.Add(tmpc2);
                result.Add(cMaxX);
                return result;
            }
            colStart += type;
        }
        cMinX.isAcitve = true;
        cMaxX.isAcitve = true;
        return null;
    }
    public void Hint(List<GridCell> blocks)
    {
        PlaySound.instance.PlayClickSound();
        foreach (GridCell tmp in blocks)
        {
            if (tmp.isAcitve)
            {
                bool isFindOut = false;
                List<GridCell> foundCells = blocks.FindAll(cell => (cell.id == tmp.id && cell.isAcitve));
                foreach (GridCell cell in foundCells)
                {
                    if (FindPaths(tmp, cell) != null)
                    {
                        tmp.isAcitve = true;
                        cell.isAcitve = true;
                        tmp.Hint();
                        cell.Hint();
                        isFindOut = true;
                        break;
                    }
                }
                if (isFindOut)
                {
                    break;
                }
            }
        }
    }
    public bool IsEnd(List<GridCell> blocks)
    {
        foreach (GridCell tmp in blocks)
        {
            if (tmp.isAcitve)
            {
                List<GridCell> foundCells = blocks.FindAll(cell => (cell.id == tmp.id && cell.isAcitve));
                foreach (GridCell cell in foundCells)
                {
                    if (FindPaths(tmp, cell) != null)
                    {
                        //Debug.LogError("Exist Couple");
                        tmp.isAcitve= true;
                        cell.isAcitve = true;
                        return false;
                    }
                }
            }
        }
        return true;
    }
}
