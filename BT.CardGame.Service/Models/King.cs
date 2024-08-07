namespace BT.CardGame.Service.Models;

public class King : Card
{
    public King(string key, CardSuit suit) : base(key, suit)
    {
    }

    public override int CardValue => 13;
}