using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultiplyIncomeEvent : MonoBehaviour
{
    private float eventDuration = 120;
    [SerializeField]
    private Button eventButton;
    [SerializeField]
    private Button _leaveEventPanelButton;
    BotController botController;
    void Start()
    {
        botController = new BotController();
        eventButton.onClick.AddListener(() => OnClickRewardAd());
    }
    private void OnClickRewardAd()
    {
        RewardAdManager manager = FindObjectOfType<RewardAdManager>();
        manager.ShowRewardedAd(() =>
        {
            DoubleIncome();
        });
    }
    private IEnumerator DoubleIncome(){
        BotSpawner.Instance.SetIncomeMultiplier(2);
        yield return new WaitForSeconds(eventDuration);
        BotSpawner.Instance.SetIncomeMultiplier(1);
    } 
}
