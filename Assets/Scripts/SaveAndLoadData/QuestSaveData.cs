using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class QuestSaveData
{
    public int Id;
    public string QuestName;
    public int CurrentAmount;
    public int TargetAmount;
    public bool IsCompleted;
    public bool IsRewardTaken;

}
