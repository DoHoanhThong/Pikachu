using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class GameManager : Singleton<GameManager>
{
    public bool isExistData;
    public bool isContinue;
    public LevelSetting[] levelSetting = new LevelSetting[8];
    //public bool isIAP;
    // Start is called before the first frame update
    void OnEnable()
    {

        //isIAP = false;
        Application.targetFrameRate = 60;
        //PlayerPrefs.DeleteAll();
        if (!File.Exists(Application.persistentDataPath + "/UserData.json"))
        {
            //Debug.LogError("not exist Data");
            isExistData = false;
        }
        else {
            //Debug.LogError("exist Data");
            isExistData = true;  
        }
        isContinue = false;
    }

}
