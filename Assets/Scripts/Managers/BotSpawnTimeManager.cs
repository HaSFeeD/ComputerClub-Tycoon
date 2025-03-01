using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotSpawnTimeManager : MonoBehaviour
{
    public static BotSpawnTimeManager Instance;
    private float _baseSpawnTime = 40f;
    private const float MinSpawnTime = 7f;

    private void Awake(){
        Instance = this;
    }
    public float GetSpawnTime(){
        return _baseSpawnTime;
    }
    public void AdjustSpawnTime(float hour){
        float clubRating = RatingManager.Instance.GetCurrentRating();
        float timeFactor = 1f;
        if(hour >= 6 && hour < 10){
            timeFactor = 1.2f;
        } 
        else if(hour >= 10 && hour < 18){
            timeFactor = 0.7f;
        } 
        else if(hour >= 18 && hour < 21){
            timeFactor = 1f;
        } 
        else if(hour >= 21){
            timeFactor = 1.5f;
        } 
        else{
            timeFactor = 1f;
        }
        float adjustedSpawnTime = _baseSpawnTime * timeFactor / (1f + clubRating);
        _baseSpawnTime = adjustedSpawnTime;
    }
}
