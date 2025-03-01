using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class BotSpawner : MonoBehaviour
{
    public static BotSpawner Instance;
    private float minBotSpawnTime = 7;
    [SerializeField] private GameObject _botPrefab;
    [SerializeField] private GameObject _spawnPoint;
    [SerializeField] private GameObject _endPoint;
    [SerializeField] private float _spawnTime = 3;
    [SerializeField] private int _botLimit = 20;
    [SerializeField] private float _reachedThreshold = 1f;
    private List<GameObject> _botList = new List<GameObject>();
    private Transform _spawnPosition;
    public float SpawnTime {private get; set;}
    public static int _botsSpawnedCount;
    private bool _isCoroutineStopped = false;
    private Coroutine spawnCoroutine;
    [Header ("Bot Starting Settings")]
    private int _botIncomeMultiplier = 1;
    private void Awake()
    {
        Instance = this;
    }
    private void Start(){
        _botsSpawnedCount = 1;
        _spawnTime = BotSpawnTimeManager.Instance.GetSpawnTime();
        SpawnTime = _spawnTime;
        _spawnPosition = _spawnPoint.GetComponent<Transform>();
        spawnCoroutine = StartCoroutine(BotSpawnerCorotine());
        _isCoroutineStopped = false;
    }

    private void Update()
    {
        DestroyBot();

        if(_botList.Count > _botLimit)
        {
            StopCoroutine(spawnCoroutine);
            _isCoroutineStopped = true;
        }
        else if(_botList.Count <= _botLimit && _isCoroutineStopped)
        {
            spawnCoroutine = StartCoroutine(BotSpawnerCorotine());
            _isCoroutineStopped = false;
        }
    }

    private void DestroyBot()
    {
        float threshold = 1f;

        for (int i = _botList.Count - 1; i >= 0; i--)
        {
            GameObject bot = _botList[i];
            if (Vector3.Distance(bot.transform.position, _endPoint.transform.position) < threshold)
            {
                _botList.RemoveAt(i);
                Destroy(bot);
            }
        }
    }


    private IEnumerator BotSpawnerCorotine()
    {
        while(true){
            yield return new WaitForSeconds(1);
            GameObject newBot = Instantiate(_botPrefab, _spawnPoint.transform.position, _spawnPoint.transform.rotation);
            BotController botController = newBot.GetComponentInChildren<BotController>();
            botController.SetBotIncomeMultiplier(_botIncomeMultiplier);
            _botList.Add(newBot);
            _botsSpawnedCount++;
            float currentSpawnTime = BotSpawnTimeManager.Instance.GetSpawnTime();
            if(currentSpawnTime < minBotSpawnTime){
                currentSpawnTime = minBotSpawnTime;
            }
            yield return new WaitForSeconds(currentSpawnTime);
        }
    }
    public static int GetBotsSpawnedCount(){
        return _botsSpawnedCount;
    }
    public void SetIncomeMultiplier(int amount){
        _botIncomeMultiplier = amount;
    }
    public void AddBotToList(GameObject bot){
        _botList.Add(bot);
        _botsSpawnedCount++;
    }
}