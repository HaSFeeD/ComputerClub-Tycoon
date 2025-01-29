public class VacateComputerAction : IBotAction
{
    private BotController bot;
    private Computer computer;

    public bool IsCompleted { get; private set; }

    public VacateComputerAction(Computer computer)
    {
        this.computer = computer;
        IsCompleted = false;
    }

    public void Initialize(BotController bot)
    {
        this.bot = bot;
        computer.Vacate(bot);
        IsCompleted = true;
    }

    public void Execute()
    {
    }
}
