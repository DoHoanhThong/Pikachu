using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class JsonSaveLoad : MonoBehaviour
{
    public void SaveToJson(List<GridCell> grid)
    {
        string path = Application.persistentDataPath + "/UserData.json";
        listDATAsave data= new listDATAsave();
        if (File.Exists(path))
        {
            //Debug.LogError("exist!");
        }
        foreach (GridCell cell in grid)
        {
            if (cell.isAcitve)
            {
                DATAsave a = new DATAsave(cell.transform.GetSiblingIndex(), cell.id);
                data.list.Add(a);
            }
            //Debug.LogError("saved key: " + cell.row + "," + cell.col);
        }
        //Debug.LogError("Count save:"+data.list.Count);
        try
        {
            string jsonToSave = JsonUtility.ToJson(data, true);
            //Debug.LogError("JSON TO SAVE: " + jsonToSave);
            File.WriteAllText(path, jsonToSave);
            //Debug.Log("Complete");
            Debug.LogError(path);
        }
        catch (Exception e)
        {
            Debug.LogError("Loi: " + e.Message);
        }
    }
    public List<DATAsave> ReadFromJson()
    {
        string path = Application.persistentDataPath + "/UserData.json";
        if (!File.Exists(path))
        {
            //Debug.LogError("not exist data!");
            return null;
        }
        string json = File.ReadAllText(path);
        listDATAsave data = JsonUtility.FromJson<listDATAsave>(json);
        return data.list;
    }
}
