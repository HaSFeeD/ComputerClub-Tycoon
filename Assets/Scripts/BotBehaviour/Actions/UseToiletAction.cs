using UnityEngine;

public class UseToiletAction : IBotAction
{
    private BotController bot;
    private float usageDuration;
    private float timer;
    private Toilet targetToilet;
    private Vector3 previousPosition;
    private Quaternion previousRotation;

    private TimerUI timerUI;

    public bool IsCompleted { get; private set; }

    public UseToiletAction(float duration)
    {
        this.usageDuration = duration;
        IsCompleted = false;
    }

    public void Initialize(BotController bot)
    {
        this.bot = bot;

        targetToilet = bot.RoomManager.FindNearestAvailableObject<Toilet>(bot.transform.position);

        if (targetToilet == null || !targetToilet.TryOccupy(bot))
        {
            Debug.Log("Немає доступних туалетів для бота " + bot.gameObject.name);
            IsCompleted = true;
            bot.CurrentActivity = BotActivity.Idle;
            return;
        }

        bot.Agent.SetDestination(targetToilet.Position);
        bot.Agent.isStopped = false;
        bot.CurrentActivity = BotActivity.Walking;
    }
    public void Execute()
{
    if (IsCompleted) return;

    float stopDist = bot.Agent.stoppingDistance;

    if (!bot.Agent.pathPending && bot.Agent.remainingDistance <= Mathf.Max(stopDist, bot.ReachedThreshold))
    {
        if (bot.CurrentActivity != BotActivity.UsingToilet)
        {
            bot.Agent.isStopped = true;
            bot.Agent.updatePosition = false;
            bot.Agent.updateRotation = false;

            previousPosition = bot.transform.position;
            previousRotation = bot.transform.rotation;

            bot.transform.position = targetToilet.Position;
            bot.transform.rotation = targetToilet.transform.rotation;

            timer = 0f;
            bot.CurrentActivity = BotActivity.UsingToilet;
        }
    }

    if (bot.CurrentActivity == BotActivity.UsingToilet)
    {
        timer += Time.deltaTime;
        if (timer >= usageDuration)
        {
            FinishUsage();
            IsCompleted = true;
            bot.CurrentActivity = BotActivity.Idle;
        }
    }
}


    private void FinishUsage()
    {
        bot.Agent.isStopped = false;
        bot.Agent.updatePosition = true;
        bot.Agent.updateRotation = true;

        bot.transform.position = previousPosition;
        bot.transform.rotation = previousRotation;

        targetToilet.Vacate(bot);

        if (timerUI != null)
        {
            GameObject.Destroy(timerUI.gameObject);
        }
    }
}
