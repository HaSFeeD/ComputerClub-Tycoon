using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Bus : MonoBehaviour
{
    private Transform _busStopPoint;
    private Transform _busEndPoint;
    private Transform _busSpawnBotPoint;
    private NavMeshAgent _busAgent;
    private float _busThreshold = 5f;
    private Coroutine spawnBotsFromBusCoroutine;
    private int _maxBotSpawnCount = 5;
    private int _botSpawnedCount = 0;
    private float _botSpawnTime = 1f;
    [SerializeField] private GameObject _botPrefab; 

    private void Awake()
    {
        _busStopPoint = PointManager.instance.busStopPoint.transform;
        _busEndPoint = PointManager.instance.busEndPoint.transform;
        _busSpawnBotPoint = PointManager.instance.busSpawnBotPoint.transform;
        _busAgent = GetComponent<NavMeshAgent>();
    }
    
    void Start()
    {
        Move(_busStopPoint);
    }

    void Update()
    {
        if (!_busAgent.isStopped && Vector3.Distance(transform.position, _busStopPoint.position) < _busThreshold)
        {
            StopBus();
        }
        
        if(_botSpawnedCount >= _maxBotSpawnCount && spawnBotsFromBusCoroutine != null)
        {
            StopCoroutine(spawnBotsFromBusCoroutine);
            spawnBotsFromBusCoroutine = null;
            Move(_busEndPoint);
        }
        
        if(Vector3.Distance(transform.position, _busEndPoint.position) < _busThreshold)
        {
            Destroy(gameObject);
        }
    }

    private void Move(Transform destinationPoint)
    {
        _busAgent.isStopped = false;
        _busAgent.SetDestination(destinationPoint.position);
    }

    private void StopBus()
    {
        if (!_busAgent.pathPending && _busAgent.remainingDistance < _busThreshold)
        {
            Debug.Log("StopBus() викликано. Автобус зупиняється.");
            _busAgent.isStopped = true;
            spawnBotsFromBusCoroutine = StartCoroutine(SpawnBot());
        }
    }


    private IEnumerator SpawnBot()
    {
        while(_botSpawnedCount < _maxBotSpawnCount)
        {
            GameObject newBot = Instantiate(_botPrefab, _busSpawnBotPoint.position, _busSpawnBotPoint.rotation);
            BotSpawner.Instance.AddBotToList(newBot);
            _botSpawnedCount++;
            yield return new WaitForSeconds(_botSpawnTime);
        }
    }
}
