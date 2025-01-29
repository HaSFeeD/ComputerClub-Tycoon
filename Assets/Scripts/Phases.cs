using System.Runtime.CompilerServices;
using UnityEngine;

[System.Serializable]

public class Phase
{
    public string Name;
    public float Duration;
    public GameObject DestinationPoint;
    public Phase (string name, float duration, GameObject destinationPoint){
        Name = name;
        Duration = duration;
        DestinationPoint = destinationPoint;
    }
    
}


