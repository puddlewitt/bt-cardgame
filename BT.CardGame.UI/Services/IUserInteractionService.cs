namespace BT.CardGame.UI.Services;

public interface IUserInteractionService
{
    string? ReadLine();
    void WriteLine(string message);
    void WriteLine(Exception ex);
}