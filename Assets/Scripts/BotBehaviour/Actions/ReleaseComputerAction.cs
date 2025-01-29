public class ReleaseComputerAction : IBotAction
{
    private BotController bot;

    public bool IsCompleted { get; private set; }

    public void Initialize(BotController bot)
    {
        this.bot = bot;
        bot.ReleaseReservedComputer();
        IsCompleted = true;
    }

    public void Execute()
    {
        
    }
}
