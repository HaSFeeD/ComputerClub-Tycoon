using UnityEngine;

public interface IUsableObject
{
    bool IsOccupied { get; }
    Vector3 Position { get; }
    BotController ReservedBy { get; }

    bool TryOccupy(BotController bot);
    void Vacate(BotController bot);
    void Reserve(BotController bot);
    void ReleaseReservation(BotController bot);
}
