using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Room : MonoBehaviour
{
    public List<Computer> Computers = new List<Computer>();
    public List<Toilet> Toilets = new List<Toilet>();
    public List<WeddingMachine> WeddingMachines = new List<WeddingMachine>();

    private void Awake()
    {
        Computers = GetComponentsInChildren<Computer>().ToList();
        Toilets = GetComponentsInChildren<Toilet>().ToList();
        WeddingMachines = GetComponentsInChildren<WeddingMachine>().ToList();
    }

    // Генеричний метод для отримання доступних об'єктів певного типу
    public List<T> GetAvailableObjects<T>() where T : MonoBehaviour, IAvailableObject
    {
        if (typeof(T) == typeof(Computer))
            return Computers as List<T>;
        if (typeof(T) == typeof(Toilet))
            return Toilets as List<T>;
        if (typeof(T) == typeof(WeddingMachine))
            return WeddingMachines as List<T>;
        return new List<T>();
    }
}
