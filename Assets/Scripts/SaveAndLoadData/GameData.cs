using System;
using System.Collections.Generic;

[Serializable]
public class GameData
{
    public float Cash;
    public int Diamonds;

    public List<int> purchasedRoomIDs;
    public GameData()
    {
        Cash = 100f;
        Diamonds = 0;
        purchasedRoomIDs = new List<int>();
    }
}
