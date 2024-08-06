namespace BT.CardGame.Service.Models;

public class Jack : Card
{
    public override int CardValue => 11;

    public Jack(string key, CardSuit suit) : base(key, suit)
    {
    }
}