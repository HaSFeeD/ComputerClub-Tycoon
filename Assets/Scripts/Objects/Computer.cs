<<<<<<< HEAD
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Computer : MonoBehaviour, IAvailableObject
{
    public bool IsOccupied {get; private set;}
    public Transform SeatPosition { get; private set; }

    public Vector3 Position => transform.position;
=======
using UnityEngine;

public class  Computer : MonoBehaviour, IUsableObject
{
    public bool IsOccupied { get; private set; }
    public Transform SeatPosition { get; private set; }
    public Vector3 Position => transform.position;
    public BotController ReservedBy { get; private set; }
>>>>>>> 27866b6 (Refactored Some Code and add new Features)

    private void Awake()
    {
        SeatPosition = transform;
    }
<<<<<<< HEAD
    public bool TryOccupy()
    {
        if (!IsOccupied)
=======

    public bool TryOccupy(BotController bot)
    {
        if (!IsOccupied && ReservedBy == bot)
>>>>>>> 27866b6 (Refactored Some Code and add new Features)
        {
            IsOccupied = true;
            return true;
        }
        return false;
    }

<<<<<<< HEAD
    public void Vacate()
    {
        IsOccupied = false;
    }
    
=======
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
>>>>>>> 27866b6 (Refactored Some Code and add new Features)
}
