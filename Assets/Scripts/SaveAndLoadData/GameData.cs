using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int Diamonds;
    public List<LevelData> Levels;
    public GameData()
    {
        Levels = new List<LevelData>();
    }
}
