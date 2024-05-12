using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridCell : MonoBehaviour
{
    [SerializeField] Color newColor;
    Coroutine a;
    [SerializeField] float timeSuggest;
    public bool isAcitve;
    [SerializeField] GameObject btn;
    [SerializeField] GameObject border;
    public int id;
    public int row;
    public int col;
    [SerializeField] bool isSuggesting;
    private void Awake()
    {
        timeSuggest = 1;
        if (btn != null)
        {
            btn.transform.GetComponent<Image>().color = new Vector4(0.9056604f, 0.8749021f, 0.8749021f, 1);
            
        }
        GridLayoutGroup gridLayoutGroup = transform.parent.GetComponent<GridLayoutGroup>();
        // Lấy index của ô trong danh sách con của GridLayoutGroup
        int index = transform.GetSiblingIndex();
        // Tính toán hàng và cột dựa trên index và kích thước của GridLayoutGroup
        row = index / 22;
        col = index % 22;
        if ((row >= 1 && row <= 9) && (col >= 1 && col <= 20))
        {
            isAcitve = true;
            return;
        }
        btn.SetActive(false);
        isAcitve = false;
    }
    
    public void Hint()
    {
        if (a != null)
        {
            StopCoroutine(a);
        }
        a = StartCoroutine(getHint());
    }
    public void OnCellClick()
    {
        //if (GameManager.instance.isIAP==true)
        //{
        //    return;
        //}

        if (GameController.instance.CD_click >= 0 || isSuggesting || GameController.instance.isMovingCell)
            return;
        GameController.instance.CD_click = 0.1f;
        PlaySound.instance.PlayClickSound();
        border.SetActive(!border.activeSelf);
        if (!border.activeSelf)
        {
            if (GameController.instance.cell1 == this.GetComponent<GridCell>())
            {
                GameController.instance.cell1 = null;
            }
            else if (GameController.instance.cell2 == this.GetComponent<GridCell>())
            {
                GameController.instance.cell2 = null;
            }
            btn.transform.GetComponent<Image>().color = new Vector4(0.9056604f, 0.8749021f, 0.8749021f, 1);
            return;
        }
        btn.transform.GetComponent<Image>().color = newColor;
        if (GameController.instance.cell1 == null)
        {
            GameController.instance.cell1 = this.GetComponent<GridCell>();
        }
        else if (GameController.instance.cell2 == null)
        {
            GameController.instance.cell2 = this.GetComponent<GridCell>();
            GameController.instance.CheckAndDrawPath();
        }

    }
    //public int ColLeft()
    //{
    //    return col - 1;
    //}
    //public int ColRight()
    //{
    //    return col + 1;
    //}
    //public int RowTop()
    //{
    //    return row - 1;
    //}
    //public int RowBottom()
    //{
    //    return row + 1;
    //}
    //public int ColTop()
    //{
    //    return col - 1;
    //}
    //public int ColBottom()
    //{
    //    return col + 1;
    //}
    public void Wrong()
    {
        btn.transform.GetComponent<Image>().color = new Vector4(0.9056604f, 0.8749021f, 0.8749021f, 1);
        border.SetActive(false);
    }
    public void Correct()
    {
        isAcitve = false;
        this.transform.GetComponent<Image>().enabled = false;
        btn.SetActive(false);
        Wrong();
    }
    public void NextLevel()
    {
        if (isAcitve) return;
        if ((row >= 1 && row <= 9) && (col >= 1 && col <= 20))
        {
            btn.SetActive(true);
            this.transform.GetComponent<Image>().enabled = true;
            isAcitve = true;
        }
    }
    IEnumerator getHint()
    {
        isSuggesting = true;
        border.SetActive(true);
        yield return new WaitForSeconds(timeSuggest);
        isSuggesting = false;
        border.SetActive(false);
    }

}
