<<<<<<< HEAD
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BotStateMachine : MonoBehaviour
{
    private BotController _botController;
    public State CurrentState { get; private set; }
=======
using UnityEngine;

public class BotStateMachine
{
    private BotController _botController;
    public BotState CurrentState { get; private set; }

>>>>>>> 27866b6 (Refactored Some Code and add new Features)
    public BotStateMachine(BotController botController)
    {
        _botController = botController;
    }
<<<<<<< HEAD
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
=======

    public void ChangeState(BotState newState)
    {
        if (CurrentState != null)
            CurrentState.Exit();

        CurrentState = newState;

        if (CurrentState != null)
            CurrentState.Enter();
    }

    public void Update()
    {
        if (CurrentState != null)
            CurrentState.Execute();
>>>>>>> 27866b6 (Refactored Some Code and add new Features)
    }
}
