using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class RewardedAdManager : MonoBehaviour
{
    private RewardedAd rewardedAd;
    private string adUnitId;
    private Action _onRewardedCallback;

    private void Start()
    {
        #if UNITY_ANDROID
            adUnitId = "ca-app-pub-xxx-androidRewardedId";
        #elif UNITY_IPHONE
            adUnitId = "ca-app-pub-xxx-iosRewardedId";
        #endif

        MobileAds.Initialize(initStatus =>
        {
            RequestRewardedAd();
        });
    }

    private void RequestRewardedAd()
    {
        AdRequest adRequest = new AdRequest();
        RewardedAd.Load(adUnitId, adRequest, (RewardedAd ad, LoadAdError error) =>
        {
            if (error != null || ad == null)
            {
                Debug.LogError("Failed to load rewarded ad: " + error);
                return;
            }
            rewardedAd = ad;
            Debug.Log("Rewarded ad loaded.");
        });
    }

    public void ShowRewardedAd(Action onRewardedCallback)
    {
        _onRewardedCallback = onRewardedCallback;

        if (rewardedAd != null && rewardedAd.CanShowAd())
        {
            rewardedAd.Show(HandleUserEarnedReward);
        }
        else
        {
            Debug.Log("Rewarded Ad is not ready yet.");
            RequestRewardedAd();
        }
    }

    private void HandleUserEarnedReward(Reward reward)
    {
        Debug.Log($"Reward Received: {reward.Type} - {reward.Amount}");
        _onRewardedCallback?.Invoke();
        RequestRewardedAd();
    }
}
