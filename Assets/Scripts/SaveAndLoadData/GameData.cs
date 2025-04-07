using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int Diamonds;
    public List<LevelData> Levels;
    public bool isAdBlockPurchased;
    public List<OfflineTimeManagers> DonateManagers;
    public GameData()
    {
        Levels = new List<LevelData>();
        DonateManagers = new List<OfflineTimeManagers>();
    }
}
