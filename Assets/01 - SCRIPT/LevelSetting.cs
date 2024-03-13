using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelSetting", menuName = "Create LevelSetting")]
public class LevelSetting : ScriptableObject
{
    public int Level = 1;
    public int Time_Second;
    public Type type;
    public int Suggestions = 5;
    public int Changes = 4;
    public enum Type
    {
        NO = 0,
        ALL_DOWN = 1,
        ALL_UP = 2,
        ALL_LEFT = 3,
        ALL_RIGHT = 4,
        DOWN_UP = 5,
        LEFT_RIGHT = 6,
        RANDOM = 7
    }
}
