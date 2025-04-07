using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvestmentsEvent : MonoBehaviour
{
    [SerializeField]
    private Button eventButton;
    [SerializeField]
    private Button _leaveEventPanelButton;
    void Start()
    {
        eventButton.onClick.AddListener(() => OnClickRewardAd());
    }
    private void OnClickRewardAd()
    {
        RewardAdManager manager = FindObjectOfType<RewardAdManager>();
        manager.ShowRewardedAd(() =>
        {
            EconomyManager.Instance.AddCash(GetRandomMoneyReward());
        });
    }
    private float GetRandomMoneyReward(){
        float cashMultiplier = Random.Range(2, 4);
        float cashCount = cashMultiplier * EconomyManager.Instance.Cash;
        return cashCount;
    }
}
