using UnityEngine;

public enum QuestType
{
    BuyRoom,
    BuyUpgrade,
    BuyObject,
    Utility
}

[CreateAssetMenu(fileName = "Quest", menuName = "ScriptableObjects/Quest", order = 1)]
public class QuestData : ScriptableObject
{
    public int QuestIndex;
    public string QuestLinkID;
    public string QuestName;
    public string QuestDescription;
    public QuestType questType;
    public int GameLevel;
    public int Reward;
    public int LevelOfObject;
    public int CountOfObjects;
    public int CurrentAmount;
    public int TargetAmount;
    public int MaxAmount = 10;
    public bool IsCompleted;
    public bool IsRewardTaken;

    public void CalculateReward()
    {
        switch (questType)
        {
            case QuestType.BuyRoom: Reward = 20; break;
            case QuestType.BuyUpgrade: Reward = 5; break;
            case QuestType.BuyObject: Reward = 10; break;
            case QuestType.Utility: Reward = 3; break;
        }
    }

    public void CheckCompletion()
    {
        if (!IsCompleted && CurrentAmount >= TargetAmount)
        {
            IsCompleted = true;
            Debug.Log($"Quest '{QuestName}' completed!");
            
        }
    }
}
