namespace BT.CardGame.Service.Models;

public class Card
{
    protected readonly string Key;

    protected Card(string key,
        CardSuit suit)
    {
        Key = key;
        Suit = suit;
    }

    public CardSuit Suit { get; set; }

    public virtual int CardValue { get; } = 0;
}