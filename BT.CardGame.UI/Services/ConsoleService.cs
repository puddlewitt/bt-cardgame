namespace BT.CardGame.UI.Services;

public class ConsoleService : IUserInteractionService
{
    public string? ReadLine() => Console.ReadLine();

    public void WriteLine(string message)
    {
        Console.WriteLine(message);
    }

    public void WriteLine(Exception ex)
    {
        Console.WriteLine(ex);
    }
}