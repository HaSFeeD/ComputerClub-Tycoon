using UnityEngine;

public class MoveAction : IBotAction
{
    private BotController bot;
    private Vector3 destination;
    private float reachedThreshold;

    public bool IsCompleted { get; private set; }

    public MoveAction(Vector3 destination, float threshold = 1f)
    {
        this.destination = destination;
        this.reachedThreshold = threshold;
        IsCompleted = false;
    }

    public void Initialize(BotController bot)
    {
        this.bot = bot;
        bot.Agent.SetDestination(destination);
        bot.Agent.isStopped = false;
        bot.CurrentActivity = BotActivity.Walking;
    }

    public void Execute()
    {
        if (!bot.Agent.pathPending && bot.Agent.remainingDistance <= reachedThreshold)
        {
            bot.Agent.isStopped = true;
            IsCompleted = true;
            bot.CurrentActivity = BotActivity.Idle;
        }
    }
}