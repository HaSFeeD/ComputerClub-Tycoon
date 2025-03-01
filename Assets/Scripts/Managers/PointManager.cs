using UnityEngine;

public class PointManager : MonoBehaviour
{
    public static PointManager instance;

    [Header ("Bot Points")]
    public GameObject spawnPoint;
    public GameObject registrationPoint;
    public GameObject queueStartPoint;
    public GameObject finishPoint;
    
    [Header ("Event Bus Point")]
    public GameObject busSpawnPoint;
    public GameObject busStopPoint;
    public GameObject busSpawnBotPoint;
    public GameObject busEndPoint;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}