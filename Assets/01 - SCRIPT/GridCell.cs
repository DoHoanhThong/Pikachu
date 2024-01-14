using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridCell : MonoBehaviour
{
    public bool isAcitve;
    [SerializeField] GameObject btn;
    [SerializeField] GameObject border;
    public int id;
    public int row;
    public int col;
    [SerializeField] bool isSelecting;
    private void Awake()
    {
        
        GridLayoutGroup gridLayoutGroup = transform.parent.GetComponent<GridLayoutGroup>();
        // Lấy index của ô trong danh sách con của GridLayoutGroup
        int index = transform.GetSiblingIndex();
        // Tính toán hàng và cột dựa trên index và kích thước của GridLayoutGroup
        row = index / gridLayoutGroup.constraintCount ;
        col = index % gridLayoutGroup.constraintCount ;
        if((row>=1 && row<=9) && (col >= 1 && col <= 20))
        {
            isAcitve = true;
            return;
        }
        isAcitve = false;
    }

    public void OnCellClick()
    {
        //PlaySound.instance.PlayClickSound();
        border.SetActive(!border.activeSelf);     
        if (!border.activeSelf)
        {
            GameController.instance.countClick -= 1;
            return;
        }
        GameController.instance.countClick += 1;
        if (GameController.instance.cell1==null)
        {
            GameController.instance.cell1 = this.GetComponent<GridCell>();
        }
        else if (GameController.instance.cell2 == null)
        {
            GameController.instance.cell2 = this.GetComponent<GridCell>();
            if (GameController.instance.countClick == 2)
            {
                GameController.instance.CheckAndDrawPath();
                GameController.instance.countClick = 0;
            }
        }
       
    }
    public int ColLeft()
    {
        return col - 1;
    }
    public int ColRight()
    {
        return col + 1;
    }
    public int RowTop()
    {
        return row - 1;
    }
    public int RowBottom()
    {
        return row + 1;
    }
    public int ColTop()
    {
        return col - 1;
    }
    public int ColBottom()
    {
        return col + 1;
    }
    public void Wrong()
    {
        border.SetActive(false);
    }
    public void Correct()
    {
        isAcitve = false;
        this.transform.GetComponent<Image>().enabled = false;
        btn.SetActive(false);
        Wrong();
    }

}
