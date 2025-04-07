using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelData
{
    public int LevelNumber;
    public bool IsLevelOpened;
    public float Cash;
    public float Rating;
    public float Income;
    public float Energy;
    public List<RoomData> rooms;
    public List<QuestSaveData> questSaveDatas;
}
