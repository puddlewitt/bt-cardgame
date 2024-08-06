namespace BT.CardGame.Service.Models;

public class Card
{
    protected readonly string Key;

    public Card(string key,
        CardSuit suit)
    {
        Key = key;
        Suit = suit;
        CardValue = 0;
    }

    public CardSuit Suit { get; }

    public virtual int CardValue { get; }
}