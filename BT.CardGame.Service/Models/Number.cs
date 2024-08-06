namespace BT.CardGame.Service.Models;

public class Number : Card
{
    public override int CardValue => int.TryParse(Key, out var valueFromKey) ? valueFromKey : 0;

    public Number(string key, CardSuit suit) : base(key, suit)
    {
    }
}