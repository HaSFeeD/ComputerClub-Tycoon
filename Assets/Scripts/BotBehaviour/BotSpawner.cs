using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class BotSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _botPrefab;
    [SerializeField] private GameObject _spawnPoint;
    [SerializeField] private GameObject _endPoint;
    [SerializeField] private float _spawnTime = 1;
    [SerializeField] private int _botLimit;
    [SerializeField] private float _reachedThreshold = 1f;
    private List<GameObject> _botList = new List<GameObject>();
    private Transform _spawnPosition;
    public float SpawnTime {private get; set;}
    public static int _botsSpawnedCount;
    
    private void Start(){
        _botsSpawnedCount = 1;
        SpawnTime = _spawnTime;
        _spawnPosition = _spawnPoint.GetComponent<Transform>();
        StartCoroutine(BotSpawnerCorotine());
    }

    private void Update(){
        if(_botList.Count > _botLimit){
            StopCoroutine(BotSpawnerCorotine());
        }
        // DestroyBot();
    }

    private IEnumerator BotSpawnerCorotine()
    {
        while(true){
<<<<<<< HEAD
            yield return new WaitForSeconds(5);
            _botList.Add(Instantiate(_botPrefab, _spawnPosition.position, _spawnPosition.rotation));
            _botsSpawnedCount =+1;
=======
            yield return new WaitForSeconds(1);
            _botList.Add(Instantiate(_botPrefab, _spawnPosition.position, _spawnPosition.rotation));
            _botsSpawnedCount += 1;
>>>>>>> 27866b6 (Refactored Some Code and add new Features)
            yield return new WaitForSeconds(SpawnTime);
        }
    }
    public static int GetBotsSpawnedCount(){
        return _botsSpawnedCount;
    }
    // private void DestroyBot(){
    //     List<GameObject> botsToRemove = new List<GameObject>();
    //     foreach(GameObject bot in _botList){
    //         if(Vector3.Distance(bot.transform.position, _endPoint.transform.position) < _reachedThreshold){
    //             botsToRemove.Add(bot);
    //         }
    //     }
    //     foreach(GameObject bot in botsToRemove){
    //         Destroy(bot);
    //         _botList.Remove(bot);
    //     }
    // }
}