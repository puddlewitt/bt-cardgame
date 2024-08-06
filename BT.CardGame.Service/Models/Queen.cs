namespace BT.CardGame.Service.Models;

public class Queen : Card
{
    public override int CardValue => 12;

    public Queen(string key, CardSuit suit) : base(key, suit)
    {
    }
}