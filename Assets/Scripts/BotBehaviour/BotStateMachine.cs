using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BotStateMachine : MonoBehaviour
{
    private BotController _botController;
    public State CurrentState { get; private set; }
    public BotStateMachine(BotController botController)
    {
        _botController = botController;
    }
    public void Initialize()
    {
        SetState(State.MovingToPoint);
    }
    public void SetState(State newState){
        CurrentState = newState;
    }
    public void UpdateState(){
        switch (CurrentState)
        {
            case State.MovingToPoint:
                break;
            case State.Registation:
                break;
            case State.WaitingAtPoint:
                break;
            case State.MovingToComputer:
                break;
            case State.UsingComputer:
                break;
            case State.MovingToToiletPoint:
                break;
            case State.UsingToilet:
                break;
            case State.MovingToWeddingMachinesPoint:
                break;
            case State.UsingWeddingMachine:
                break;
            case State.MovingToFinishPoint:
                break;
            case State.Finished:
                break;
        }
    }

}
