using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FreeDiamonds : MonoBehaviour
{
    private const int coolDownDuration = 3700;
    private int coolDownTimeExpired;
    public Button watchAdForDiamondsButton;
    public TextMeshProUGUI watchAdForDiamondsText;
    [SerializeField]
    private Sprite onCoolDownSprite;
    [SerializeField]
    private Sprite watchADSprite;
    void Start()
    {
        watchAdForDiamondsButton.onClick.AddListener(() => OnClickRewardAd());
        if(PlayerPrefs.HasKey("LastFreeDiamondsRewardTime")){
            long binaryTime = long.Parse(PlayerPrefs.GetString("LastFreeDiamondsRewardTime"));
            System.DateTime time = System.DateTime.FromBinary(binaryTime);
            int elapsedTime = (int)(System.DateTime.UtcNow - time).TotalSeconds;
            coolDownTimeExpired = Mathf.Max(coolDownDuration - elapsedTime, 0);
        }
        else{
            coolDownTimeExpired = 0;
        }
        if(coolDownTimeExpired > 0){
            WaitForCoolDown();
        }
        UpdateCoolDown(coolDownTimeExpired);
    }
    public void UpdateCoolDown(int seconds){
        if(seconds > 0){
            watchAdForDiamondsButton.interactable = false;
            watchAdForDiamondsText.text = ConvertTime(seconds).ToString();
            watchAdForDiamondsButton.image.sprite = onCoolDownSprite;

        }
        else{
            watchAdForDiamondsButton.interactable = true;
            watchAdForDiamondsText.text = "";
            watchAdForDiamondsButton.image.sprite = watchADSprite;
        }
    }
        public string ConvertTime(int seconds)
    {
        if (seconds >= 3600)
        {
            int hours = seconds / 3600;
            int minutes = (seconds % 3600) / 60;
            return string.Format("{0:D2}:{1:D2}", hours, minutes);
        }
        else
        {
            int minutes = seconds / 60;
            int secs = seconds % 60;
            return string.Format("{0:D2}:{1:D2}", minutes, secs);
        }
    }

    public async Task WaitForCoolDown(){
        while(coolDownTimeExpired > 0){
            UpdateCoolDown(coolDownTimeExpired);
            await Task.Delay(1000);
            if(this == null) {
                return;
            }
            coolDownTimeExpired--;
        }
        UpdateCoolDown(coolDownTimeExpired);
    }

        private void OnClickRewardAd()
    {
        RewardAdManager manager = FindObjectOfType<RewardAdManager>();
        manager.ShowRewardedAd(() =>
        {
            EconomyManager.Instance.AddDiamonds(60);
            PlayerPrefs.SetString("LastFreeDiamondsRewardTime", System.DateTime.UtcNow.ToBinary().ToString());
            coolDownTimeExpired = coolDownDuration;
            WaitForCoolDown();
        });
    }
    
}
