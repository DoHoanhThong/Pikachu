using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class GameController : Singleton<GameController>
{
    //Action
    public static System.Action onHomeClick;
    //public static System.Action Next_Level;
    //private
    int rows, cols;
    Coroutine a;
    bool isLose = false;
    bool isWinning = false;
    bool isChanging;
    bool isSuggesting;
    List<int> used;
    List<int> usedBG;

    //SerializeField
    [SerializeField] MoveCell moveCell;
    [SerializeField] int time, begin;
    [SerializeField] LoadSavePP pp;
    [SerializeField] Text winText;
    [SerializeField] Image bg, change, hint;
    [SerializeField] JsonSaveLoad json;
    [SerializeField] Image cd;
    [SerializeField] FindPath findPath;
    [SerializeField] GameObject WinPanel;
    [SerializeField] GameObject LosePanel;
    [SerializeField] DrawPath drawPath;
    [SerializeField] int currentCouple = 0;
    //public
    public bool isMovingCell;
    public float CD_click;
    public GridCell cell1, cell2;
    public GridLayoutGroup gridLayoutGroup;
    public LineRenderer lineRenderer;
    public List<GridCell> blocks = new List<GridCell>();
    //end

    // Start is called before the first frame update
    void Start()
    {
        BtnOnClick.isResume += Resume;
        BtnOnClick.isPause += Pause;
        Application.quitting += Quit;
        onHomeClick += Save;
        usedBG = new List<int>();
        int tmp = Random.Range(1, 7); usedBG.Add(tmp);
        bg.sprite = Resources.Load<Sprite>("BG/" + tmp);
        CD_click = 0;
        used = new List<int>();
        rows = 11;
        cols = 22;
        if (GameManager.instance.isContinue)
        {
            GameManager.instance.isContinue = false;
            LoadExistDATA();
            if (PlayerPrefs.GetInt("Changes") == 0)
            {

                StartCoroutine(CheckEnd());
            }
        }
        else
        {
            PlayerPrefs.SetInt("Time", GameManager.instance.levelSetting[0].Time_Second);
            RandomSprite();
        }
        time = PlayerPrefs.GetInt("Time");
        int level = PlayerPrefs.GetInt("Level");
        begin = GameManager.instance.levelSetting[(level < 8 && level >= 1) ? (level - 1) : 7].Time_Second;
        cd.fillAmount = (float)time / begin;
        a = StartCoroutine(CountDown());
    }
    private void OnDestroy()
    {
        BtnOnClick.isResume -= Resume;
        BtnOnClick.isPause -= Pause;
        onHomeClick -= Save;
        Application.quitting -= Quit;
    }
    private void Quit()
    {
        PlayerPrefs.SetInt("HasDATA", 1);
        Save();
    }
    void LoadExistDATA()
    {
        List<DATAsave> list = json.ReadFromJson();
        List<int> inDATAsave = new List<int>();
        for (int i = 0; i < list.Count; i++)
        {
            inDATAsave.Add(list[i].index);
        }
        for (int i = 0; i < blocks.Count; i++)
        {
            if (inDATAsave.Contains(blocks[i].transform.GetSiblingIndex()))
            {
                blocks[i].isAcitve = true;
                int index = inDATAsave.IndexOf(blocks[i].transform.GetSiblingIndex());
                blocks[i].id = list[index].id;
                used.Add(blocks[i].id);
                blocks[i].transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Blocks/" + list[index].id);
            }
            else
            {
                blocks[i].Correct();
            }
        }
        currentCouple = list.Count / 2;
    }
    private void Update()
    {
        CD_click -= Time.deltaTime;
    }
    IEnumerator CountDown()
    {
        while (time > 0)
        {
            time -= 1;
            cd.fillAmount -= (float)1 / begin;
            yield return new WaitForSeconds(1);
        }
        Lose();
    }
    void RandomSprite()
    {
        used = new List<int>();
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
        int total = (isChanging) ? currentCouple * 2 - 1 : blocks.Count - 1;
        while (assigned.Count < total)
        {
            Sprite randomSprite = Resources.Load<Sprite>("Blocks/" + used[count]);
            int index = 0;

            //1
            do
            {
                index = Random.Range(0, blocks.Count);
            }
            while (assigned.Contains(index) || !blocks[index].isAcitve);
            assigned.Add(index);

            //2
            int index2 = 0;
            do
            {
                index2 = Random.Range(0, blocks.Count);
            }
            while (assigned.Contains(index2) || !blocks[index2].isAcitve);
            assigned.Add(index2);

            blocks[index].transform.GetChild(0).GetComponent<Image>().sprite = randomSprite;
            blocks[index].id = used[count];

            blocks[index2].id = used[count];
            blocks[index2].transform.GetChild(0).GetComponent<Image>().sprite = randomSprite;
            count++;
        }
        if (isChanging)
            return;
        currentCouple = assigned.Count / 2;
    }
    public void Hint()
    {
        if (isSuggesting)
            return;
        if (PlayerPrefs.GetInt("Hint") == 0)
        {
            return;
        }
        isSuggesting = true;
        StartCoroutine(WaitToSuggestAgain());
        findPath.Hint(blocks);
        pp.Hint();
        
    }
    public void NextLevel()
    {

        pp.LevelUp();

        if (WinPanel.activeSelf)
        {
            WinPanel.SetActive(false);
        }
        if (a != null)
        {
            StopCoroutine(a);
        }
        NewLevel();
    }
    public void Change()
    {
        if (isChanging)
            return;
        if (PlayerPrefs.GetInt("Changes") == 0)
        {
            return;
        }
        if (PlayerPrefs.GetInt("Changes") == 1)
        {
            StartCoroutine(CheckEnd());
        }
        PlaySound.instance.PlayRandomSound();
        isChanging = true;
        StartCoroutine(WaitToChangeAgain());
        RandomBlocks();
        pp.Changes();
    }
    public void Save()
    {
        if (isWinning)
        {
            NextLevel();
        }
        else if (!isLose)
        {
            PlayerPrefs.SetInt("Time", time);
        }
        else
        {
            NewLevel();
        }

        json.SaveToJson(blocks);
    }
    public void CheckAndDrawPath()
    {
        if (cell1.id != cell2.id)
        {
            WrongChoice();
            return; // Giá trị không giống nhau, không kết nối
        }
        List<GridCell> path = findPath.FindPaths(cell1, cell2);
        if (path != null && path.Count > 0)
        {
            CorrectChoice();
            drawPath.Draw(path);
        }
        else
        {
            WrongChoice();
        }
    }

    void CorrectChoice()
    {
        PlaySound.instance.PlayCorrectSound();
        currentCouple -= 1;
        cell1.Correct();
        cell2.Correct();
        StartCoroutine(Move());

        StartCoroutine(timeDrawCD());
        if (currentCouple == 0)
        {
            PlaySound.instance.PlayWINSound();
            Win();
        }
    }
    void Win()
    {
        isWinning = true;
        WinPanel.SetActive(true);
        winText.transform.DOScale(new Vector2(0.5f, 0.5f), 0.5f).OnComplete(() =>
        {
            winText.transform.DOScale(new Vector2(1.5f, 1.5f), 0.5f).OnComplete(() =>
            {
                winText.transform.DOScale(Vector2.one, 0.5f);
            });
        });
    }
    void Lose()
    {
        isLose = true;
        if (!WinPanel.activeSelf && !LosePanel.activeSelf)
        {
            LosePanel.SetActive(true);
        }
        PlaySound.instance.PlayGOverSound();
    }
    void WrongChoice()
    {
        cell1.Wrong();
        cell2.Wrong();
        cell1 = null;
        cell2 = null;
        PlaySound.instance.PlayWrongSound();
    }
    IEnumerator timeDrawCD()
    {
        yield return new WaitForSeconds(0.3f);
        drawPath.ResetLine();
    }
    public void Pause()
    {
        //Debug.LogError("pause");

        //if(GameManager.instance.isIAP==true)
        //{
        //    return;
        //}
        if (a != null)
        {
            StopCoroutine(a);
        }
    }
    public void Resume()
    {
        //Debug.LogError("resume");
        a = StartCoroutine(CountDown());
    }
    public void Restart()
    {
        if (isLose)
        {
            isLose = false;
        }
        if (WinPanel.activeSelf)
        {
            WinPanel.SetActive(false);
        }
        pp.ReStart();
        if (LosePanel.activeSelf)
        {
            LosePanel.SetActive(false);
        }
        NewLevel();
    }
    void NewLevel()
    {
        PlaySound.instance.PlayClickSound();
        blocks = new List<GridCell>();
        for (int i = 0; i < gridLayoutGroup.transform.childCount; i++)
        {
            GridCell tmp = gridLayoutGroup.transform.GetChild(i).GetComponent<GridCell>();
            if ((tmp.row >= 1 && tmp.row <= 9) && (tmp.col >= 1 && tmp.col <= 20))
            {
                tmp.NextLevel();
                blocks.Add(tmp);
            }
        }

        int level = PlayerPrefs.GetInt("Level");
        begin = GameManager.instance.levelSetting[(level < 8 && level >= 1) ? (level - 1) : 7].Time_Second;
        time = begin;
        cd.fillAmount = 1;
        a = StartCoroutine(CountDown());
        RandomSprite();

        int index = 0;
        if (usedBG.Count == 6)
        {
            usedBG.Clear();
        }
        do
        {
            index = Random.Range(1, 7);
        }
        while (usedBG.Contains(index));
        usedBG.Add(index);
        bg.sprite = Resources.Load<Sprite>("BG/" + index);
    }
    IEnumerator CheckEnd()
    {
        while (time > 0)
        {
            if (findPath.IsEnd(blocks))
            {
                Lose();
                //Debug.LogError("isEnd");
                break;
            }
            yield return new WaitForSeconds(10);
        }
    }
    IEnumerator Move()
    {
        isMovingCell = true;
        yield return new WaitForSeconds(0.3f);
        moveCell.MoveByLevel(cell1, cell2, gridLayoutGroup);

        isMovingCell = false;
        cell2 = null;
        cell1 = null;
    }
    IEnumerator WaitToChangeAgain()
    {
        float tmpTime = 4;
        change.fillAmount = 0;
        while (tmpTime > 0)
        {
            float newTime = Time.deltaTime;
            change.fillAmount += newTime/4;
            tmpTime -= newTime;
            //Debug.LogError(tmpTime);
            yield return new WaitForSeconds(newTime);
            
        }
        change.fillAmount = 1;
        isChanging = false;
    }
    IEnumerator WaitToSuggestAgain()
    {
        float tmpTime = 1;
        hint.fillAmount = 0;
        while (tmpTime > 0)
        {
            float newTime = Time.deltaTime; 
            hint.fillAmount +=  newTime;
            tmpTime -= newTime;
            yield return new WaitForSeconds(newTime);
            
        }
        hint.fillAmount = 1;
        isSuggesting = false;
    }
}

