namespace BT.CardGame.UI.Services;

public interface ICardGameScoreService
{
    Task GoAsync(CancellationToken ct);
}