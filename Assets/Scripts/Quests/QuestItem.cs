using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI QuestTitleText;
    [SerializeField] private TextMeshProUGUI QuestDescriptionText;
    [SerializeField] private TextMeshProUGUI QuestAimCount;
    [SerializeField] private TextMeshProUGUI QuestReward;
    [SerializeField] private Button QuestCompletedButton;
    [SerializeField] private Image questRewardImage;
    [SerializeField] private Sprite DiamondsSprite;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float animationSpeed = 10f;
    
    private QuestData _questData;
    private int questReward;

    public void Start()
    {
        UpdateQuestItem();
    }

    public void CreateQuestItem(string questTitleText, string questDescriptionText, string questAimCount, int questReward, bool IsCompleted, QuestData questData)
    {
        _questData = questData;
        QuestTitleText.text = questTitleText;
        QuestDescriptionText.text = questDescriptionText;
        QuestAimCount.text = questAimCount;
        QuestReward.text = questReward.ToString();
        this.questReward = questReward;
        QuestCompletedButton.interactable = IsCompleted && !_questData.IsRewardTaken;
        questRewardImage.sprite = DiamondsSprite;
        QuestCompletedButton.onClick.RemoveAllListeners();
        QuestCompletedButton.onClick.AddListener(GetReward);
    }

    public void UpdateQuestItem()
    {
        QuestAimCount.text = $"{_questData.CurrentAmount} / {_questData.TargetAmount}";
        QuestCompletedButton.interactable = _questData.IsCompleted && !_questData.IsRewardTaken;
    }

    public QuestData GetQuestData()
    {
        return _questData;
    }

    private void GetReward()
    {
        EconomyManager.Instance.AddDiamonds(questReward);
        _questData.IsRewardTaken = true;
        GameManager.Instance.SaveQuest(
            _questData.QuestIndex,
            _questData.QuestName,
            _questData.CurrentAmount,
            _questData.TargetAmount,
            _questData.IsCompleted,
            _questData.IsRewardTaken
            );
        QuestManager.Instance.QuestsLastIndex++;
        UpdateQuestData();

        if (_questData.CurrentAmount < _questData.MaxAmount)
        {
            QuestManager.Instance.AllQuests.Add(_questData);
        }
        StartCoroutine(QuestCompletingAnimation());
        
    }

    private void UpdateQuestData()
    {
        if (_questData.CurrentAmount < _questData.MaxAmount)
        {
            _questData.QuestIndex += 5;
            _questData.TargetAmount++;
            _questData.IsCompleted = false;
            _questData.IsRewardTaken = false;
        }
    }

    public IEnumerator QuestCreatingAnimation()
    {
        while (canvasGroup.alpha < 1f)
        {
            canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, 1f, animationSpeed * Time.deltaTime);
            yield return null;
        }
    }

    public IEnumerator QuestCompletingAnimation()
    {
        canvasGroup.alpha = 1f;
        while (canvasGroup.alpha > 0f)
        {
            canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, 0f, animationSpeed * Time.deltaTime);
            yield return null;
        }
        QuestManager.Instance.RemoveQuest(gameObject);
        Destroy(gameObject);
    }

}
