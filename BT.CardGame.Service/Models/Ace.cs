namespace BT.CardGame.Service.Models;

public class Ace : Card
{
    public override int CardValue => 14;

    public Ace(string key, CardSuit suit) : base(key, suit)
    {
    }
}