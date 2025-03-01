using UnityEngine;

public class Computer : MonoBehaviour, IUsableObject
{
    [SerializeField]
    private GameObject chair;
    public bool IsOccupied { get; private set; }
    public Transform SeatPosition { get; private set; }
    public Vector3 Position => transform.position;
    public BotController ReservedBy { get; private set; }
    public int RoomID { get; set; }

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
}
