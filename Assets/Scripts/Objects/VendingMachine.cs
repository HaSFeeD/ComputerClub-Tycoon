using UnityEngine;

public class VendingMachine : MonoBehaviour, IUsableObject
{
    public bool IsOccupied { get; private set; }
    public Vector3 Position => transform.position;
    public BotController ReservedBy { get; private set; }

    public bool TryOccupy(BotController bot)
    {
        if (!IsOccupied)
        {
            IsOccupied = true;
            ReservedBy = bot;
            return true;
        }
        return false;
    }

    public void Vacate(BotController bot)
    {
        if (ReservedBy == bot)
        {
            IsOccupied = false;
            ReservedBy = null;
        }
    }

    public void Reserve(BotController bot)
    {
    }

    public void ReleaseReservation(BotController bot)
    {
    }
}
