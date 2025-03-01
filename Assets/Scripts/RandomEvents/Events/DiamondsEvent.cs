using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiamondsEvent : MonoBehaviour
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
        RewardedAdManager manager = FindObjectOfType<RewardedAdManager>();
        manager.ShowRewardedAd(() =>
        {
            EconomyManager.Instance.AddDiamonds(GetRandomDiamondsReward());
        });
    }
    private int GetRandomDiamondsReward(){
        int diamondsCount = Random.Range(5, 10);
        return diamondsCount;
    }
}
