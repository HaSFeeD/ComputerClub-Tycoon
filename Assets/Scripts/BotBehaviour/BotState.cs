using UnityEngine;

public abstract class BotState
{
    protected BotController botController;
    protected BotStateMachine stateMachine;

    protected BotState(BotController controller, BotStateMachine machine)
    {
        botController = controller;
        stateMachine = machine;
    }

    public virtual void Enter() { }
    public virtual void Execute() { }
    public virtual void Exit() { }
}
