using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SaveLoadManager
{
    private static string fileName = "save.json";

    public static void SaveData(GameData data)
    {
        string json = JsonUtility.ToJson(data);
        string path = Path.Combine(Application.persistentDataPath, fileName);

        File.WriteAllText(path, json);
        Debug.Log("Saved to: " + path);
    }

    public static GameData LoadData()
    {
        string path = Path.Combine(Application.persistentDataPath, fileName);

        if (!File.Exists(path))
        {
            Debug.LogWarning("No save file found at " + path);
            return null;
        }

        string json = File.ReadAllText(path);
        GameData data = JsonUtility.FromJson<GameData>(json);
        return data;
    }
    public static void ResetData(){
        string path = Path.Combine(Application.persistentDataPath, fileName);
        if (!File.Exists(path))
        {
            Debug.LogWarning("No save file found at " + path);
            return;
        }
        GameData newDefault = new GameData();
        string defaultJson = JsonUtility.ToJson(newDefault);
        File.WriteAllText(path, defaultJson);
    }
}
