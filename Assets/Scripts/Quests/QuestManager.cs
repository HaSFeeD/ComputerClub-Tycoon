using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;
    public event System.Action OnInitialized;
    private const int MaxQuestsInList = 3;
    public int QuestsLastIndex = 0;
    public List<QuestData> AllQuests;
    private GameObject[] activeQuests = new GameObject[MaxQuestsInList];
    [SerializeField] private GameObject questItemPrefab;
    [SerializeField] private GameObject questContainer;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        AllQuests = Resources.LoadAll<QuestData>("Quests")
            .Where(ch => ch.GameLevel == GameManager.Instance.LevelNumber)
            .OrderBy(ch => ch.QuestIndex)
            .ToList();

        QuestsLastIndex = AllQuests.Count;
        UpdateQuestMenu();

        OnInitialized?.Invoke();
    }

    public void UpdateQuestProgress(QuestType questType, string LinkID, int amount = 1)
    {
        foreach (GameObject questObj in activeQuests)
        {
            if (questObj != null)
            {
                QuestItem qi = questObj.GetComponent<QuestItem>();
                if (qi != null)
                {
                    QuestData qData = qi.GetQuestData();
                    if (qData.questType == questType && qData.QuestLinkID == LinkID)
                    {
                        qData.CurrentAmount += amount;
                        qData.CheckCompletion();
                        qi.UpdateQuestItem();
                        GameManager.Instance.SaveQuest(
                            qData.QuestIndex,
                            qData.QuestName,
                            qData.CurrentAmount,
                            qData.TargetAmount,
                            qData.IsCompleted,
                            qData.IsRewardTaken
                        );
                    }
                }
            }
        }
        foreach (QuestData quest in AllQuests)
        {
            if (quest.questType == questType && quest.QuestLinkID == LinkID)
            {
                quest.CurrentAmount += amount;
                quest.CheckCompletion(); 
                GameManager.Instance.SaveQuest(
                    quest.QuestIndex,
                    quest.QuestName,
                    quest.CurrentAmount,
                    quest.TargetAmount,
                    quest.IsCompleted,
                    quest.IsRewardTaken
                );                       
            }
        }
        UpdateQuestMenu();
    }

    public void UpdateQuestMenu()
{
    // Спочатку повертаємо в загальний список всі квести з активних слотів
    for (int i = 0; i < MaxQuestsInList; i++)
    {
        if (activeQuests[i] != null)
        {
            QuestData existingQuest = activeQuests[i].GetComponent<QuestItem>().GetQuestData();
            AllQuests.Add(existingQuest);
            Destroy(activeQuests[i]);
            activeQuests[i] = null;
        }
    }

    // Беремо виконані квести без нагороди першими
    var questsToShow = AllQuests
        .Where(q => q.IsCompleted && !q.IsRewardTaken)
        .OrderBy(q => q.QuestIndex)
        .ToList();

    // Додаємо інші незавершені квести
    questsToShow.AddRange(AllQuests
        .Where(q => !q.IsCompleted && !q.IsRewardTaken)
        .OrderBy(q => q.QuestIndex));

    int addedQuestsCount = 0;

    // Заповнюємо активні слоти першими трьома доступними квестами
    foreach (QuestData questData in questsToShow)
    {
        if (addedQuestsCount >= MaxQuestsInList) break;

        GameObject questObj = Instantiate(questItemPrefab, questContainer.transform);
        QuestItem questItem = questObj.GetComponent<QuestItem>();
        questItem.CreateQuestItem(
            questData.QuestName,
            questData.QuestDescription,
            $"{questData.CurrentAmount}/{questData.TargetAmount}",
            questData.Reward,
            questData.IsCompleted,
            questData
        );

        StartCoroutine(questItem.QuestCreatingAnimation());
        activeQuests[addedQuestsCount] = questObj;
        AllQuests.Remove(questData);

        addedQuestsCount++;
    }
}


    public void RemoveQuest(GameObject quest)
    {
        for (int i = 0; i < activeQuests.Length; i++)
        {
            if (activeQuests[i] == quest)
            {
                activeQuests[i] = null;
            }
        }
        UpdateQuestMenu();
    }
    public QuestData FindQuestByName(string name)
    {
        foreach (var quest in AllQuests)
        {
            if (quest.QuestName == name)
            {
                return quest;
            }
        }
        foreach (var questObj in activeQuests)
        {
            if (questObj != null)
            {
                QuestData quest = questObj.GetComponent<QuestItem>().GetQuestData();
                if (quest.QuestName == name)
                {
                    return quest;
                }
            }
        }
        return null;
    }


}
