using BT.CardGame.Service.Models;

namespace BT.CardGame.Service.Tests.Models;

public class CardTests
{
    [TestCase("2", ExpectedResult = 2)]
    [TestCase("-1", ExpectedResult = -1)]
    public int CardValue_ShouldBeParsed_WhenIntegerKeyProvided(string key)
    {
        var card = new Number(key, CardSuit.Club);

        return card.CardValue;
    }

    [TestCase("ABC", ExpectedResult = 0)]
    [TestCase("", ExpectedResult = 0)]
    public int CardValue_ShouldDefaultToZero_WhenUnableToParseKey(string key)
    {
        var card = new Number(key, CardSuit.Club);

        return card.CardValue;
    }
    
    [TestCase(CardSuit.None, ExpectedResult = CardSuit.None)]
    [TestCase(CardSuit.Heart, ExpectedResult = CardSuit.Heart)]
    public CardSuit Suit_ShouldBeStored_WhenInjectedViaConstructor(CardSuit cardSuit)
    {
        var card = new Number("2", cardSuit);

        return card.Suit;
    }

    [TestCase("2", ExpectedResult = 0)]
    [TestCase("-1", ExpectedResult = 0)]
    public int CardValue_ShouldBeZero_WhenSetViaConstructor(string key)
    {
        var card = new Card(key, CardSuit.Club);

        return card.CardValue;
    }
}