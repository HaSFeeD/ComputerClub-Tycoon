using UnityEngine;

public interface IAvailableObject
{
    bool IsOccupied { get; }
    bool TryOccupy();
    void Vacate();
    Vector3 Position { get; }
}
