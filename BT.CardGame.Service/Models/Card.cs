using BT.CardGame.Service.Services;

namespace BT.CardGame.Service.Models;

public class Card
{
    public CardSuit Suit { get; set; }
    public CardType Type { get; set; }
    public int Value { get; set; }
}