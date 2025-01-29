using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BotController : MonoBehaviour
{
<<<<<<< HEAD
    [SerializeField] private float _tempBalance = 100;
    [SerializeField] private float _reachedThreshold = 1f;
    [SerializeField] private GameObject _timerGameObject;

    private Timer _clock;
    private float _timer = 0;
    int playtimeDuration = 0;
    private int _currentPhaseIndex = 0;
    private Vector3 _previousPosition;
    private Quaternion _previousRotation;

    private NavMeshAgent _agent;
    private List<Phase> phases;

    private Computer _targetComputer;
    private Toilet _targetToilet;
    private WeddingMachine _targetWeddingMachine;
    private RoomManager _roomManager;
    private GameManager _gameManager;

    private bool _hasComputerReserved = false;
    private bool _hasToiletReserved = false;

    // Булеві змінні для анімацій
    public bool isMovingToPoint { get; private set; } = false;
    public bool isWaitingAtPoint { get; private set; } = false;
    public bool isMovingToComputer { get; private set; } = false;
    public bool isUsingComputer { get; private set; } = false;
    public bool isFinished { get; private set; } = false;
    public Vector3 seatOffset;


    private State _currentState;
    private float _endReachedThreshold = 10;
=======
    [SerializeField] private float _tempBalance = 100f;
    [SerializeField] private float _reachedThreshold = 0.1f;
    [SerializeField] private GameObject _timerUIPrefab; 

    private NavMeshAgent _agent;
    private RoomManager _roomManager;

    public Vector3 seatOffset;

    public NavMeshAgent Agent => _agent;
    public float ReachedThreshold => _reachedThreshold;
    public Vector3 SeatOffset => seatOffset;
    public int PlaytimeDuration { get; private set; }
    public float TempBalance => _tempBalance;
    public RoomManager RoomManager => _roomManager;
    public GameObject TimerUIPrefab => _timerUIPrefab;

    public BotActivity CurrentActivity { get; set; } = BotActivity.Idle;

    private Queue<IBotAction> actionQueue = new Queue<IBotAction>();
    private IBotAction currentAction;

    public Computer ReservedComputer { get; private set; }
>>>>>>> 27866b6 (Refactored Some Code and add new Features)

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        if (_agent == null)
        {
            Debug.LogError("NavMeshAgent не знайдено на " + gameObject.name);
        }
    }

    private void Start()
    {
<<<<<<< HEAD
        playtimeDuration = Random.Range(1,6);
        _clock = _timerGameObject.GetComponent<Timer>();
        if (_clock == null)
        {
            Debug.LogError("Компонент Timer не знайдено на _timerGameObject");
        }

        phases = PointManager.instance._points;
        if (phases == null || phases.Count == 0)
        {
            Debug.LogError("Список фаз пустий у PointManager");
        }
=======
        PlaytimeDuration = Random.Range(5, 15);
>>>>>>> 27866b6 (Refactored Some Code and add new Features)

        _roomManager = FindObjectOfType<RoomManager>();
        if (_roomManager == null)
        {
            Debug.LogError("RoomManager не знайдено у сцені.");
        }

<<<<<<< HEAD
        _gameManager = FindObjectOfType<GameManager>();
        if (_gameManager == null)
        {
            Debug.LogError("GameManager не знайдено у сцені.");
        }

        if (phases != null && phases.Count > 0)
        {
            SetState(State.MovingToPoint);
            MoveToPoint(_currentPhaseIndex);
        }
=======
        EnqueueActions();
>>>>>>> 27866b6 (Refactored Some Code and add new Features)
    }

    private void Update()
    {
<<<<<<< HEAD
        if(Vector2.Distance(gameObject.transform.position, PointManager.instance.finishPoint.transform.position ) < _endReachedThreshold){
            Destroy(gameObject);
            return;
        }
        switch (_currentState)
        {
            case State.MovingToPoint:
                CheckArrivalAtRegistration();
                break;
            case State.WaitingAtPoint:
                _timer += Time.deltaTime;
                CheckTimer();
                break;
            case State.MovingToComputer:
                CheckArrivalAtComputer();
                break;
            case State.UsingComputer:
                _timer += Time.deltaTime;
                CheckUsageTimer();
                break;
            case State.MovingToFinishPoint:
                CheckArrivalAtFinishPoint();
                break;
            case State.Finished:
                Destroy(gameObject);
                return; // Додаємо return, щоб зупинити виконання
        }
    }


    private void SetState(State newState)
    {
        // Скидаємо всі булеві змінні
        isMovingToPoint = false;
        isWaitingAtPoint = false;
        isMovingToComputer = false;
        isUsingComputer = false;
        isFinished = false;

        _currentState = newState;

        // Встановлюємо відповідну булеву змінну
        switch (_currentState)
        {
            case State.MovingToPoint:
                isMovingToPoint = true;
                break;
            case State.WaitingAtPoint:
                isWaitingAtPoint = true;
                break;
            case State.MovingToComputer:
                isMovingToComputer = true;
                break;
            case State.UsingComputer:
                isUsingComputer = true;
                break;
            case State.Finished:
                isFinished = true;
                break;
        }
    }

    private void CheckArrivalAtRegistration()
    {
        if (!_agent.pathPending && _agent.remainingDistance <= _reachedThreshold)
        {
            if (_currentPhaseIndex >= phases.Count)
            {
                // Бот досяг FinishPoint
                SetState(State.Finished);
            }
            else
            {
                Phase currentPhase = phases[_currentPhaseIndex];
                if (currentPhase.Name == "Registration")
                {
                    OnRegistration();
                }
                else
                {
                    // Інші фази, якщо є
                    _agent.isStopped = true;
                    _timer = 0;
                    _clock.SetDuration(currentPhase.Duration).Begin();
                    SetState(State.WaitingAtPoint);
                }
            }
        }
    }

    private void OnRegistration()
    {
        _gameManager.AddBalance(_tempBalance * playtimeDuration);
        _agent.isStopped = true;
        _timer = 0;
        _clock.SetDuration(phases[_currentPhaseIndex].Duration).Begin();
        SetState(State.WaitingAtPoint);
    }

    private void CheckTimer()
    {
        Phase currentPhase = phases[_currentPhaseIndex];
        if (_timer >= currentPhase.Duration)
        {
            _clock.End();
            _timer = 0;
            _agent.isStopped = false;

            if (!_hasComputerReserved)
            {
                UseComputer();
            }
            else
            {
                MoveToNextPhase();
            }
        }
    }

    private void UseComputer()
    {
        if (_roomManager != null)
        {
            _targetComputer = _roomManager.FindNearestAvailableObject<Computer>(transform.position);
            if (_targetComputer != null && _targetComputer.TryOccupy())
            {
                _agent.SetDestination(_targetComputer.SeatPosition.position);
                _agent.isStopped = false;
                _hasComputerReserved = true;
                SetState(State.MovingToComputer);
            }
            else
            {
                // Немає доступних комп'ютерів
                Debug.Log("Немає доступних комп'ютерів для бота " + gameObject.name);
                MoveToNextPhase();
            }
        }
    }

    private void CheckArrivalAtComputer()
    {
        if (!_agent.pathPending && _agent.remainingDistance <= _reachedThreshold)
        {
            // Зупиняємо агента та вимикаємо оновлення
            _agent.isStopped = true;
            _agent.updatePosition = false;
            _agent.updateRotation = false;

            // Скидаємо швидкість агента
            _agent.velocity = Vector3.zero;

            // Зберігаємо попередню позицію та оберт бота
            _previousPosition = transform.position;
            _previousRotation = transform.rotation;

            // Встановлюємо позицію та оберт бота на позицію стільця
            if (_targetComputer != null && _targetComputer.SeatPosition != null)
            {
                seatOffset = new Vector3(-0.5f, -1.7f, 1); // Налаштуйте зсув за потреби
                transform.position = _targetComputer.SeatPosition.position + seatOffset;
                Quaternion rotationOffset = Quaternion.Euler(0, 90, 0);
                transform.rotation = _targetComputer.SeatPosition.rotation * rotationOffset;
            }
            else
            {
                Debug.LogError("SeatPosition не знайдено у цільового комп'ютера.");
            }

            // Починаємо анімацію використання комп'ютера
            _timer = 0;
            _clock.SetDuration(playtimeDuration).Begin();
            SetState(State.UsingComputer);
        }
    }
    private void FinishUsingComputer()
    {
        _clock.End();
        _agent.isStopped = false;
        _agent.updatePosition = true;
        _agent.updateRotation = true;

        // Відновлюємо попередню позицію та оберт бота
        transform.position = _previousPosition;
        transform.rotation = _previousRotation;

        // Звільняємо комп'ютер
        if (_hasComputerReserved)
        {
            _targetComputer.Vacate();
            _hasComputerReserved = false;
        }

        MoveToNextPhase();
    }
    private void UseToilet()
    {
        if (_roomManager != null)
        {
            _targetToilet = _roomManager.FindNearestAvailableObject<Toilet>(transform.position);
            if (_targetToilet != null && _targetToilet.TryOccupy())
            {
                _agent.SetDestination(_targetToilet.ToiletPosition.position);
                _agent.isStopped = false;
                _hasComputerReserved = true;
                SetState(State.MovingToComputer);
            }
            else
            {
                // Немає доступних комп'ютерів
                Debug.Log("Немає доступних туалетів для бота, він злий " + gameObject.name);
                MoveToFinishPoint();
            }
        }
    }
    private void CheckArrivalAtToilet()
    {
        if (!_agent.pathPending && _agent.remainingDistance <= _reachedThreshold)
        {
            // Зупиняємо агента та вимикаємо оновлення
            _agent.isStopped = true;
            _agent.updatePosition = false;
            _agent.updateRotation = false;

            // Скидаємо швидкість агента
            _agent.velocity = Vector3.zero;

            // Зберігаємо попередню позицію та оберт бота
            _previousPosition = transform.position;
            _previousRotation = transform.rotation;

            // Встановлюємо позицію та оберт бота на позицію стільця
            if (_targetToilet != null && _targetToilet.ToiletPosition != null)
            {
                seatOffset = new Vector3(-0.5f, -1.7f, 1); // Налаштуйте зсув за потреби
                transform.position = _targetToilet.ToiletPosition.position + seatOffset;
                Quaternion rotationOffset = Quaternion.Euler(0, 90, 0);
                transform.rotation = _targetToilet.ToiletPosition.rotation * rotationOffset;
            }
            else
            {
                Debug.LogError("SeatPosition не знайдено у цільового туалету.");
            }

            // Починаємо анімацію використання комп'ютера
            _timer = 0;
            _clock.SetDuration(playtimeDuration).Begin();
            SetState(State.UsingToilet);
        }
    }
    private void FinishUsingToilet()
    {
        _clock.End();
        _agent.isStopped = false;
        _agent.updatePosition = true;
        _agent.updateRotation = true;

        // Відновлюємо попередню позицію та оберт бота
        transform.position = _previousPosition;
        transform.rotation = _previousRotation;

        // Звільняємо комп'ютер
        if (_hasToiletReserved)
        {
            _targetToilet.Vacate();
            _hasToiletReserved = false;
        }

        MoveToNextPhase();
    }


    private void CheckUsageTimer()
    {
        // Перевіряємо, чи минув час використання комп'ютера
        if (_timer >= _clock.Duration)
        {
            _clock.End();
            _timer = 0;
            FinishUsingComputer();
        }
    }


    private void MoveToNextPhase()
    {
        _currentPhaseIndex++;

        if (_currentPhaseIndex >= phases.Count)
        {
            MoveToFinishPoint();
        }
        else
        {
            MoveToPoint(_currentPhaseIndex);
        }
    }

    private void MoveToPoint(int phaseIndex)
    {
        Phase currentPhase = phases[phaseIndex];
        if (currentPhase.DestinationPoint != null)
        {
            _agent.SetDestination(currentPhase.DestinationPoint.transform.position);
            _agent.isStopped = false;
            SetState(State.MovingToPoint);
        }
        else
        {
            Debug.LogError("DestinationPoint є null у фазі: " + currentPhase.Name);
        }
    }

    private void MoveToFinishPoint()
    {
        GameObject finishPoint = PointManager.instance.finishPoint;
        if (finishPoint != null)
        {
            _agent.SetDestination(finishPoint.transform.position);
            _agent.isStopped = false;
            SetState(State.MovingToFinishPoint);
=======
        if (currentAction == null || currentAction.IsCompleted)
        {
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

    private void EnqueueActions()
    {
        actionQueue.Enqueue(new MoveAction(PointManager.instance.spawnPoint.transform.position, _reachedThreshold));

        actionQueue.Enqueue(new MoveAction(PointManager.instance.registrationPoint.transform.position, _reachedThreshold));
        actionQueue.Enqueue(new RegistrationAction());

        ReservedComputer = _roomManager.FindAvailableComputer();
        if (ReservedComputer != null)
        {
            ReservedComputer.Reserve(this);
            actionQueue.Enqueue(new UseComputerAction(PlaytimeDuration, ReservedComputer));
        }
        else
        {
            Debug.Log("Немає доступних комп'ютерів для бота " + gameObject.name);
            actionQueue.Enqueue(new MoveAction(PointManager.instance.finishPoint.transform.position, _reachedThreshold));
            return;
        }

        bool willTakeBreak = Random.value < 0.7f;
        if (willTakeBreak)
        {
            actionQueue.Enqueue(new VacateComputerAction(ReservedComputer));
            
            float rand = Random.value;
            if (rand < 0.5f)
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

        GameObject finishPoint = PointManager.instance.finishPoint;
        if (finishPoint != null)
        {
            actionQueue.Enqueue(new MoveAction(finishPoint.transform.position, _reachedThreshold));
>>>>>>> 27866b6 (Refactored Some Code and add new Features)
        }
        else
        {
            Debug.LogError("FinishPoint не призначено в PointManager");
<<<<<<< HEAD
            SetState(State.Finished);
        }
    }
    private void CheckArrivalAtFinishPoint()
    {
        if (!_agent.pathPending && _agent.remainingDistance <= _reachedThreshold)
        {
            SetState(State.Finished);
=======
        }
    }

    public void ReleaseReservedComputer()
    {
        if (ReservedComputer != null)
        {
            ReservedComputer.ReleaseReservation(this);
            ReservedComputer = null;
>>>>>>> 27866b6 (Refactored Some Code and add new Features)
        }
    }
}
