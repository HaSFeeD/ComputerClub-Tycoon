using UnityEngine;

public class WeddingMachine : MonoBehaviour, IAvailableObject
{
    public bool IsOccupied { get; private set; }
    public Transform ToiletPosition { get; private set; }
    public Vector3 Position => transform.position;
    private void Awake()
    {
        ToiletPosition = transform;
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
