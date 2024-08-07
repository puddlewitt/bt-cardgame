namespace BT.CardGame.Service.Models;

public class Ace : Card
{
    public Ace(string key, CardSuit suit) : base(key, suit)
    {
    }

    public override int CardValue => 14;
}