using UnityEngine;

public class QueueRegistrationAction : IBotAction
{
    private BotController bot;
    private bool joinedQueue = false;

    public bool IsRegistered { get; private set; } = false;

    private float registrationDuration = 1f;
    private float remainingTime;

    private float arrivalThreshold = 0.5f;

    public bool IsCompleted { get; private set; }

    public void Initialize(BotController bot)
    {
        this.bot = bot;
        if (!QueueManager.instance.TryJoinQueue(bot))
        {
            IsRegistered = false;
            bot.Agent.isStopped = false;
            bot.Agent.SetDestination(PointManager.instance.finishPoint.transform.position);
            return;
        }
        IsRegistered = true;
        joinedQueue = true;
        remainingTime = registrationDuration;
    }

    public void Execute()
    {
        if (!IsRegistered)
        {
            float distanceToFinish = Vector3.Distance(bot.transform.position, bot.Agent.destination);
            if (distanceToFinish <= arrivalThreshold)
            {
                IsCompleted = true;
            }
            return;
        }
        if (!joinedQueue || IsCompleted) return;

        QueueManager.instance.UpdateQueuePositions();

        float distanceToDest = Vector3.Distance(bot.transform.position, bot.Agent.destination);

        if (distanceToDest > arrivalThreshold)
        {
            bot.CurrentActivity = BotActivity.Walking;
            bot.Agent.updateRotation = true;
            return;
        }

        bot.CurrentActivity = BotActivity.Waiting;
        bot.Agent.updateRotation = false;
        bot.transform.rotation = Quaternion.Euler(0, 180, 0);

        if (QueueManager.instance.IsBotAtFront(bot))
        {
            remainingTime -= Time.deltaTime;

            if (remainingTime <= 0)
            {
                QueueManager.instance.RemoveBotFromQueue(bot);

                bot.Agent.isStopped = false;
                bot.Agent.updateRotation = true;
                bot.CurrentActivity = BotActivity.Walking;

                IsCompleted = true;
            }
        }
    }
}
