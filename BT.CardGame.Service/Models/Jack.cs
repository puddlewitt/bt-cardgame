namespace BT.CardGame.Service.Models;

public class Jack : Card
{
    public Jack(string key, CardSuit suit) : base(key, suit)
    {
    }

    public override int CardValue => 11;
}