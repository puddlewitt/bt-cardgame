namespace BT.CardGame.Service.Models;

public class Queen : Card
{
    public Queen(string key, CardSuit suit) : base(key, suit)
    {
    }

    public override int CardValue => 12;
}