using UnityEngine;

public class UseVendingMachineAction : IBotAction
{
    private BotController bot;
    private float usageDuration;
    private float timer;
    private VendingMachine targetMachine;
    private Vector3 previousPosition;
    private Quaternion previousRotation;

    private TimerUI timerUI;

    public bool IsCompleted { get; private set; }

    public UseVendingMachineAction(float duration)
    {
        this.usageDuration = duration;
        IsCompleted = false;
    }

    public void Initialize(BotController bot)
    {
        this.bot = bot;

        targetMachine = bot.RoomManager.FindNearestAvailableObject<VendingMachine>(bot.transform.position);

        if (targetMachine == null || !targetMachine.TryOccupy(bot))
        {
            Debug.Log("Немає доступних автоматів для бота " + bot.gameObject.name);
            IsCompleted = true;
            bot.CurrentActivity = BotActivity.Idle;
            return;
        }

        bot.Agent.SetDestination(targetMachine.Position);
        bot.Agent.isStopped = false;
        bot.CurrentActivity = BotActivity.Walking;
    }

    public void Execute()
    {
        if (IsCompleted) return;

        if (!bot.Agent.pathPending && bot.Agent.remainingDistance <= bot.ReachedThreshold)
        {
            if (bot.CurrentActivity != BotActivity.UsingVendingMachine)
            {
                bot.Agent.isStopped = true;

                previousPosition = bot.transform.position;
                previousRotation = bot.transform.rotation;

                Vector3 direction = (targetMachine.Position - bot.transform.position).normalized;
                bot.transform.rotation = Quaternion.LookRotation(direction);

                timer = 0f;
                bot.CurrentActivity = BotActivity.UsingVendingMachine;

                //timerUI = GameObject.Instantiate(bot.TimerUIPrefab, bot.transform).GetComponent<TimerUI>();
                //timerUI.SetDuration(usageDuration);
                //timerUI.Begin();
            }
        }

        if (bot.CurrentActivity == BotActivity.UsingVendingMachine)
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

        bot.transform.position = previousPosition;
        bot.transform.rotation = previousRotation;

        targetMachine.Vacate(bot);

        if (timerUI != null)
        {
            GameObject.Destroy(timerUI.gameObject);
        }
    }
}
