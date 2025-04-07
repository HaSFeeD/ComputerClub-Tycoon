using UnityEngine;

public class Computer : MonoBehaviour, IUsableObject
{
    [SerializeField] public int ID;
    [SerializeField] public string QuestLink;
    [SerializeField] public bool IsPurchased;
    public bool IsOccupied { get; private set; }
    public Transform SeatPosition { get; private set; }
    public Vector3 Position => transform.position;
    public BotController ReservedBy { get; private set; }
    public int RoomID { get; set; }
    [SerializeField] private GameObject chair;

    private void Awake()
    {
        SeatPosition = chair.transform;
    }

    public bool TryOccupy(BotController bot)
    {
        if (!IsOccupied && ReservedBy == bot)
        {
            IsOccupied = true;
            return true;
        }
        return false;
    }

    public void Vacate(BotController bot)
    {
        if (ReservedBy == bot)
        {
            IsOccupied = false;
        }
    }

    public void Reserve(BotController bot)
    {
        if (ReservedBy == null)
        {
            ReservedBy = bot;
        }
    }

    public void ReleaseReservation(BotController bot)
    {
        if (ReservedBy == bot)
        {
            IsOccupied = false;
            ReservedBy = null;
        }
    }
    public void SetPurchased(bool value, Computer computer)
    {
        IsPurchased = value;
        RoomManager.Instance.Computers.Add(computer);
    }
}
