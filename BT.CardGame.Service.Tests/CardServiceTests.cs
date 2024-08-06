using BT.CardGame.Service.Services;

namespace BT.CardGame.Service.Tests;

public class CardServiceTests
{
    private ICardService _cardService;
    
    [SetUp]
    public void Setup()
    {
        _cardService = new CardService();
    }

    [TestCase("2C", ExpectedResult = 20)]
    [TestCase("2D", ExpectedResult = 40)]
    [TestCase("2H", ExpectedResult = 60)]
    [TestCase("2S", ExpectedResult = 80)]
    [TestCase("TC", ExpectedResult = 100)]
    [TestCase("JC", ExpectedResult = 110)]
    [TestCase("QC", ExpectedResult = 120)]
    [TestCase("KC", ExpectedResult = 130)]
    [TestCase("AC", ExpectedResult = 140)]
    [TestCase("3C,4C", ExpectedResult = 7)]
    [TestCase("TC,TD,TH,TS", ExpectedResult = 100)]
    public int CalculateScore_ShouldCalculateScore_WhenSingleOrMultipleCardsUsed(string cards)
    {
        var response = _cardService.CalculateScore(cards);

        Assert.That(response.ErrorMessage, Is.EqualTo(string.Empty));
        
        return response.Score;
    }
    
    [TestCase("JK", ExpectedResult = 0)]
    [TestCase("JK,JK", ExpectedResult = 0)]
    [TestCase("2C,JK", ExpectedResult = 4)]
    [TestCase("JK,2C,JK", ExpectedResult = 8)]
    [TestCase("TC,TD,JK,TH,TS", ExpectedResult = 200)]
    [TestCase("TC,TD,JK,TH,TS,JK", ExpectedResult = 400)]
    public int CalculateScore_ShouldDoubleTheScore_WhenJokerIsUsed(string cards)
    {
        var response = _cardService.CalculateScore(cards);

        Assert.That(response.ErrorMessage, Is.EqualTo(string.Empty));
        
        return response.Score;
    }

    [TestCase("1S", ExpectedResult = "Card not recognised")]
    [TestCase("2B", ExpectedResult = "Card not recognised")]
    [TestCase("2S,1S", ExpectedResult = "Card not recognised")]
    [TestCase("3H,3H", ExpectedResult = "Cards cannot be duplicated")]
    [TestCase("4D,5D,4D", ExpectedResult = "Cards cannot be duplicated")]
    [TestCase("JK,JK,JK", ExpectedResult = "A hand cannot contain more than two Jokers")]
    [TestCase("2S|3D", ExpectedResult = "Invalid input string")]
    public string CalculateScore_ShouldReturnErrorMessage_WhenInvalidCardsUsed(string cards)
    {
        var response = _cardService.CalculateScore(cards);

        Assert.That(response.Score, Is.EqualTo(0));

        return response.ErrorMessage;
    }
}