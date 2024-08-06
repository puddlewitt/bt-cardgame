namespace BT.CardGame.Service.Services;

public interface ICardService
{
    public (int Score, string ErrorMessage) CalculateScore(string cards);
}