using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class listDATAsave
{
    public List<DATAsave> list=new List<DATAsave> ();
}
[System.Serializable]
public class DATAsave 
{
    public int index;
    public int id;
    public DATAsave(int index, int id)
    {
        this.index = index;
        this.id = id;
    }
}
