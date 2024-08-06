namespace BT.CardGame.Service.Models;

public class Joker : Card
{
    public override int CardValue => 0;

    public Joker(string key) : base(key, CardSuit.None)
    {
    }
}