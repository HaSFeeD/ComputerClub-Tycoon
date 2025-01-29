using UnityEngine;

<<<<<<< HEAD
public class Toilet : MonoBehaviour, IAvailableObject
{
    public bool IsOccupied { get; private set; }
    public Transform ToiletPosition { get; private set; }
    public Vector3 Position => transform.position;
    private void Awake()
    {
        ToiletPosition = transform;
    }

    public bool TryOccupy()
=======
public class Toilet : MonoBehaviour, IUsableObject
{
    public bool IsOccupied { get; private set; }
    public Vector3 Position => transform.position;
    public BotController ReservedBy { get; private set; }

    public bool TryOccupy(BotController bot)
>>>>>>> 27866b6 (Refactored Some Code and add new Features)
    {
        if (!IsOccupied)
        {
            IsOccupied = true;
<<<<<<< HEAD
=======
            ReservedBy = bot;
>>>>>>> 27866b6 (Refactored Some Code and add new Features)
            return true;
        }
        return false;
    }

<<<<<<< HEAD
    public void Vacate()
    {
        IsOccupied = false;
=======
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
>>>>>>> 27866b6 (Refactored Some Code and add new Features)
    }
}
