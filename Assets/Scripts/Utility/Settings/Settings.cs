using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    public static Settings Instance;
    [Header ("Game background music")]
    public AudioSource gameBackgroundMusic;
    public bool IsMusicOn = true;
    private void Awake()
    {
        Instance = this;
        if(PlayerPrefs.HasKey("MusicSettings")){
            IsMusicOn = PlayerPrefs.GetInt("MusicSettings") == 1;
        }

        if(IsMusicOn){
            gameBackgroundMusic.mute = false;
            
        }
        else{
            gameBackgroundMusic.mute = true;
        }
    }
}
