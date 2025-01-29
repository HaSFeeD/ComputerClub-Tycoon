using UnityEngine;

public class WaitAction : IBotAction
{
    private BotController bot;
    private float duration;
    private float timer;

    private TimerUI timerUI; 

    public bool IsCompleted { get; private set; }

    public WaitAction(float duration)
    {
        this.duration = duration;
        IsCompleted = false;
    }

    public void Initialize(BotController bot)
    {
        this.bot = bot;
        bot.Agent.isStopped = true;
        timer = 0f;

        bot.CurrentActivity = BotActivity.Waiting;

        timerUI = GameObject.Instantiate(bot.TimerUIPrefab, bot.transform).GetComponent<TimerUI>();
        timerUI.SetDuration(duration);
        timerUI.Begin();
    }

    public void Execute()
    {
        timer += Time.deltaTime;
        if (timer >= duration)
        {
            IsCompleted = true;
            bot.CurrentActivity = BotActivity.Idle;
            if (timerUI != null)
            {
                GameObject.Destroy(timerUI.gameObject);
            }
        }
    }
}
