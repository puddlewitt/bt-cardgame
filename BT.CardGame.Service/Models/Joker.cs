namespace BT.CardGame.Service.Models;

public class Joker : Card
{
    public Joker(string key) : base(key, CardSuit.None)
    {
    }

    public override int CardValue => 0;
}