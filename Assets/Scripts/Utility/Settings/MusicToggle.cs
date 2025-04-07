using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicToggle : MonoBehaviour
{
    private Button musicButton;
    [SerializeField]
    private Sprite buttonTurnedOff;
    [SerializeField]
    private Sprite buttonTurnedOn;
    private static AudioSource audioSource;
    private static bool isMusicOn;

    void Start()
    {
        audioSource = Settings.Instance.gameBackgroundMusic;
        isMusicOn = Settings.Instance.IsMusicOn;
        musicButton = GetComponentInChildren<Button>();
        if(isMusicOn){
            musicButton.image.sprite = buttonTurnedOff;
        }
        else{
            musicButton.image.sprite = buttonTurnedOn;
        }
        musicButton.onClick.AddListener(() => {
            if(isMusicOn){
                MusicTurnOff();
            }
            else{
                MusicTurnOn();
            }
            Settings.Instance.IsMusicOn = isMusicOn;
        });
    }

    private void MusicTurnOn(){
        audioSource.mute = false;
        isMusicOn = true;
        musicButton.image.sprite = buttonTurnedOff;
        PlayerPrefs.SetInt("MusicSettings", 1);
    }
    
    private void MusicTurnOff(){
        audioSource.mute = true;
        isMusicOn = false;
        musicButton.image.sprite = buttonTurnedOn;
        PlayerPrefs.SetInt("MusicSettings", 0);
    }
}
