using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotSpawnTimeManager : MonoBehaviour
{
    public static BotSpawnTimeManager Instance;
    private const float InitialSpawnTime = 40f;
    private float _baseSpawnTime;
    private float _spawnTimeWithoutTime;
    private const float MinSpawnTime = 7f;
    

    private void Awake(){
        Instance = this;
        _baseSpawnTime = InitialSpawnTime;
    }
    
    public float GetSpawnTime(){
        return _baseSpawnTime;
    }
    
    public float GetAdditionalSpawnTime(){
        float clubRating = RatingManager.Instance.GetCurrentRating();
        float adjustedSpawnTime = InitialSpawnTime * 2f / (1f + clubRating);
        return adjustedSpawnTime;
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
        float adjustedSpawnTime = InitialSpawnTime * timeFactor / (1f + clubRating);
        _baseSpawnTime = Mathf.Max(adjustedSpawnTime, MinSpawnTime);
        Debug.Log(_baseSpawnTime);
    }
}
