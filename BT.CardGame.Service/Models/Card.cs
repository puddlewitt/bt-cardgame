namespace BT.CardGame.Service.Models;

public class Card
{
    public CardSuit Suit { get; set; }
    public CardType CardType { get; set; }
    public int CardValue { get; set; }
}