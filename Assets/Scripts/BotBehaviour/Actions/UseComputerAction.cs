using UnityEngine;

public class UseComputerAction : IBotAction
{
    private BotController bot;
    private float usageDuration;
    private float timer;
    private Computer targetComputer;
    private Vector3 previousPosition;
    private Quaternion previousRotation;

    private TimerUI timerUI;

    public bool IsCompleted { get; private set; }

    public UseComputerAction(float duration, Computer computer)
    {
        this.usageDuration = duration;
        this.targetComputer = computer;
        IsCompleted = false;
    }

    public void Initialize(BotController bot)
    {
        this.bot = bot;
        if (targetComputer != null && targetComputer.TryOccupy(bot))
        {
            bot.Agent.SetDestination(targetComputer.SeatPosition.position);
            bot.Agent.isStopped = false;
            bot.CurrentActivity = BotActivity.Walking;
        }
        else
        {
            IsCompleted = true;
            bot.CurrentActivity = BotActivity.Idle;
        }
    }

    public void Execute()
    {
        if (IsCompleted) return;

        if (!bot.Agent.pathPending && bot.Agent.remainingDistance <= bot.ReachedThreshold)
        {
            if (bot.CurrentActivity != BotActivity.UsingComputer)
            {
                bot.Agent.isStopped = true;
                bot.Agent.updatePosition = false;
                bot.Agent.updateRotation = false;

                previousPosition = bot.transform.position;
                previousRotation = bot.transform.rotation;

                bot.transform.position = targetComputer.SeatPosition.position + new Vector3(0, -1f, 0);
                bot.transform.rotation = targetComputer.SeatPosition.rotation * Quaternion.Euler(0, 90, 0);

                timer = 0f;
                bot.CurrentActivity = BotActivity.UsingComputer;

                // timerUI = GameObject.Instantiate(bot.TimerUIPrefab, bot.transform).GetComponent<TimerUI>();
                // timerUI.SetDuration(usageDuration);
                // timerUI.Begin();
            }
        }

        if (bot.CurrentActivity == BotActivity.UsingComputer)
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

        targetComputer.Vacate(bot);

        if (timerUI != null)
        {
            GameObject.Destroy(timerUI.gameObject);
        }
    }
}