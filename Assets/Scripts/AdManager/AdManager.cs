using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class AdManager : MonoBehaviour
{
    private InterstitialAd interstitialAd;
    private float _adTimer = 300f;
    private float _timePassed = 0f;
    private bool _isAdBlockerExist = true;

    private string adUnitId;
    
    void Start()
    {
        if(_isAdBlockerExist){
            Destroy(this);
        }
        #if UNITY_ANDROID
            adUnitId = "ca-app-pub-xxx-androidInterstitalId";
        #elif UNITY_IPHONE
            adUnitId = "ca-app-pub-xxx-iosInterstitialId";
        #else
            adUnitId = "unexpected_platform";
        #endif
        MobileAds.Initialize(initStatus => { RequestInterstitial(); });
    }

    void Update()
    {
        _timePassed += Time.deltaTime;
        if (_timePassed >= _adTimer && interstitialAd != null && interstitialAd.CanShowAd()) 
        {
            interstitialAd.Show();
            _timePassed = 0f; 
        }
    }

    private void RequestInterstitial()
    {
        AdRequest adRequest = new AdRequest();
        InterstitialAd.Load(adUnitId, adRequest, (InterstitialAd ad, LoadAdError error) =>
        {
            if (error != null || ad == null)
            {
                Debug.Log("Failed to load interstitial ad: " + error);
                return;
            }

            interstitialAd = ad;
        });
    }
}
