using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class BotController : MonoBehaviour
{
    [SerializeField] private float _reachedThreshold = 0.1f;
    [SerializeField] private GameObject _timerUIPrefab; 

    private NavMeshAgent _agent;
    private RoomManager _roomManager;

    public Vector3 seatOffset;

    public NavMeshAgent Agent => _agent;
    public float ReachedThreshold => _reachedThreshold;
    public Vector3 SeatOffset => seatOffset;
    public int PlaytimeDuration { get; private set; }
    public RoomManager RoomManager => _roomManager;
    public GameObject TimerUIPrefab => _timerUIPrefab;

    public BotActivity CurrentActivity { get; set; } = BotActivity.Idle;

    private Queue<IBotAction> actionQueue = new Queue<IBotAction>();
    private IBotAction currentAction;

    public Computer ReservedComputer { get; private set; }
    public QueuePoint AssignedQueuePoint { get; set; }
    public int BotIncomeMultiplier = 1;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        _roomManager = FindObjectOfType<RoomManager>();

        PlaytimeDuration = Random.Range(1, 10); 

        EnqueueInitialActions();
    }

    private void Update()
    {
        if (currentAction == null || currentAction.IsCompleted)
        {
            if (currentAction is QueueRegistrationAction registrationAction)
            {
                if (registrationAction.IsRegistered)
                {
                    EnqueueComputerActions();
                }
            }

            if (actionQueue.Count > 0)
            {
                currentAction = actionQueue.Dequeue();
                currentAction.Initialize(this);
            }
            else
            {
                Destroy(gameObject);
                return;
            }
        }

        currentAction.Execute();
    }
    
    private void EnqueueInitialActions()
    {
        actionQueue.Enqueue(new MoveAction(PointManager.instance.spawnPoint.transform.position, _reachedThreshold));
        actionQueue.Enqueue(new QueueRegistrationAction());
    }

    private void EnqueueComputerActions()
    {
        ReservedComputer = _roomManager.FindAvailableComputer();
        if (ReservedComputer != null)
        {
            if (PlaytimeDuration > 0)
            {
                var room = RoomManager.Instance.FindRoomByID(ReservedComputer.RoomID);
                Debug.Log($"Adding cash: income = {room._roomIncome}, PlaytimeDuration = {PlaytimeDuration}, Multiplier = {BotIncomeMultiplier}");
                EconomyManager.Instance.AddCash(((room._roomIncome * PlaytimeDuration) * BotIncomeMultiplier) + IncomeManager.Instance.GetBonusIncome());
            }
            ReservedComputer.Reserve(this);
            actionQueue.Enqueue(new UseComputerAction(PlaytimeDuration, ReservedComputer));
            
            bool willTakeBreak = Random.value < 1f;
            if (willTakeBreak)
            {
                actionQueue.Enqueue(new VacateComputerAction(ReservedComputer));
                if (Random.value < 0.9f)
                {
                    actionQueue.Enqueue(new UseToiletAction(Random.Range(2f, 5f)));
                }
                else
                {
                    actionQueue.Enqueue(new UseVendingMachineAction(Random.Range(2f, 5f)));
                }
                actionQueue.Enqueue(new UseComputerAction(Random.Range(3, 10), ReservedComputer));
            }
            
            actionQueue.Enqueue(new ReleaseComputerAction());
            if (PointManager.instance.finishPoint != null)
            {
                actionQueue.Enqueue(new MoveAction(PointManager.instance.finishPoint.transform.position, _reachedThreshold));
            }
        }
        else
        {
            if (PointManager.instance.finishPoint != null)
            {
                actionQueue.Enqueue(new MoveAction(PointManager.instance.finishPoint.transform.position, _reachedThreshold));
            }
        }
    }

    public void ReleaseReservedComputer()
    {
        if (ReservedComputer != null)
        {
            ReservedComputer.ReleaseReservation(this);
            ReservedComputer = null;
        }
    }
    
    public void ClearActionQueue()
    {
        actionQueue.Clear();
    }
    public void SetBotIncomeMultiplier(int multiplier){
        BotIncomeMultiplier = multiplier;
    }
}
