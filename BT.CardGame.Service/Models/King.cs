namespace BT.CardGame.Service.Models;

public class King : Card
{
    public override int CardValue => 13;

    public King(string key, CardSuit suit) : base(key, suit)
    {
    }
}