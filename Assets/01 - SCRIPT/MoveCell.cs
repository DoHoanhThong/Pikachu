using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Unity.Burst.Intrinsics.X86.Avx;

public class MoveCell : MonoBehaviour
{
    public void MoveByLevel(GridCell cell1, GridCell cell2, GridLayoutGroup gridLayoutGroup)
    {

        int Level = PlayerPrefs.GetInt("Level");
        switch (Level)
        {
            case 1:
                break;
            case 2:
                MoveVertical(cell1, gridLayoutGroup, 1);
                MoveVertical(cell2, gridLayoutGroup, 1);
                break;
            case 3:
                MoveVertical(cell1, gridLayoutGroup, -1);
                MoveVertical(cell2, gridLayoutGroup, -1);
                break;
            case 4:
                MoveHorizontal(cell1, gridLayoutGroup, -1);
                MoveHorizontal(cell2, gridLayoutGroup, -1);
                break;
            case 5:
                MoveHorizontal(cell1, gridLayoutGroup, 1);
                MoveHorizontal(cell2, gridLayoutGroup, 1);
                break;
            default:
                int random = Random.Range(-1, 1);
                if (random == -1)
                {
                    int random2 = Random.Range(0, 100);
                    MoveVertical(cell1, gridLayoutGroup, (random2 < 50) ? -1 : 1);
                }
                else
                {
                    int random2 = Random.Range(0, 100);
                    MoveHorizontal(cell1, gridLayoutGroup, (random2 < 50) ? 1 : -1);
                }
                random = Random.Range(-1, 1);
                if (random == -1)
                {
                    int random2 = Random.Range(0, 100);
                    MoveVertical(cell2, gridLayoutGroup, (random2 < 50) ? -1 : 1);
                }
                else
                {
                    int random2 = Random.Range(0, 100);
                    MoveHorizontal(cell2, gridLayoutGroup, (random2 < 50) ? 1 : -1);
                }
                break;
        }
    }
    public void MoveVertical(GridCell cell, GridLayoutGroup grid, int type)
    {
        //type=1 down,=-1:up
        int index = cell.transform.GetSiblingIndex();
        while (cell.row >= 1 && cell.row <= 9)
        {
            GridCell up = grid.transform.GetChild(index - 22 * type).GetComponent<GridCell>();
            int tmpROW = up.row;
            int tmpINDEX = up.transform.GetSiblingIndex();
            //set
            up.row = cell.row;
            up.transform.SetSiblingIndex(index);

            cell.row = tmpROW;
            cell.transform.SetSiblingIndex(tmpINDEX);
            index = tmpINDEX;
        }
    }
    public void MoveHorizontal(GridCell cell, GridLayoutGroup grid, int type)
    {
        //type=1 right,=-1 left;
        int index = cell.transform.GetSiblingIndex();
        while (cell.col >= 1 && cell.col <= 20)
        {
            GridCell up = grid.transform.GetChild(index - 1 * type).GetComponent<GridCell>();
            int tmpCOL = up.col;
            int tmpINDEX = up.transform.GetSiblingIndex();
            //set
            up.col = cell.col;
            up.transform.SetSiblingIndex(index);

            cell.col = tmpCOL;
            cell.transform.SetSiblingIndex(tmpINDEX);
            index = tmpINDEX;
        }
    }
}
