using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Computer : MonoBehaviour
{
    public bool IsOccupied {get; private set;}
    public Transform SeatPosition;
    private void Awake(){
        SeatPosition = gameObject.transform;
    }
    public bool TryOccupy()
    {
        if (!IsOccupied)
        {
            IsOccupied = true;
            return true;
        }
        return false;
    }

    public void Vacate()
    {
        IsOccupied = false;
    }
    
}
