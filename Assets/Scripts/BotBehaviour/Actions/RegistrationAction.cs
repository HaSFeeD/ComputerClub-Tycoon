using UnityEngine;

public class RegistrationAction : IBotAction
{
    private BotController bot;

    public bool IsCompleted { get; private set; }

    public void Initialize(BotController bot)
    {
        this.bot = bot;
        bot.Agent.isStopped = true;

        EconomyManager.Instance.AddCash(bot.TempBalance * bot.PlaytimeDuration);

        IsCompleted = true; 
        bot.CurrentActivity = BotActivity.Idle;
    }

    public void Execute()
    {
        
    }
}
