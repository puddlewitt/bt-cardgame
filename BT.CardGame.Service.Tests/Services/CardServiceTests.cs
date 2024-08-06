using BT.CardGame.Service.Services;

namespace BT.CardGame.Service.Tests.Services;

public class CardServiceTests
{
    private ICardService _cardService;
    
    [SetUp]
    public void Setup()
    {
        _cardService = new CardService();
    }

    [TestCase("2C", ExpectedResult = 2)]
    [TestCase("2D", ExpectedResult = 4)]
    [TestCase("2H", ExpectedResult = 6)]
    [TestCase("2S", ExpectedResult = 8)]
    [TestCase("TC", ExpectedResult = 10)]
    [TestCase("JC", ExpectedResult = 11)]
    [TestCase("QC", ExpectedResult = 12)]
    [TestCase("KC", ExpectedResult = 13)]
    [TestCase("AC", ExpectedResult = 14)]
    [TestCase("3C,4C", ExpectedResult = 7)]
    [TestCase("TC,TD,TH,TS", ExpectedResult = 100)]
    [TestCase("6C", ExpectedResult = 6)]
    [TestCase("7C", ExpectedResult = 7)]
    [TestCase("8C", ExpectedResult = 8)]
    [TestCase("9C", ExpectedResult = 9)]
    public int CalculateScore_ShouldCalculateScoreWithoutErrors_WhenSingleOrMultipleCardsUsed(string cards)
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
    public int CalculateScore_ShouldDoubleTheScoreWithoutErrors_WhenJokerIsUsed(string cards)
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
    public string CalculateScore_ShouldReturnErrorMessageWithoutScore_WhenInvalidCardsUsed(string cards)
    {
        var response = _cardService.CalculateScore(cards);

        Assert.That(response.Score, Is.EqualTo(0));

        return response.ErrorMessage;
    }

    [Test]
    public void CalculateScore_ShouldCalculateAPositiveScore_WhenAllBasicCombinationsWithoutJokerUsed(
        [Values("2", "3", "4", "5", "6", "7", "8", "9", "T", "J", "K", "Q", "A")] string value,
        [Values("H", "D", "C", "S")] string suit)
    {
        var card = $"{value}{suit}";

        var response = _cardService.CalculateScore(card);

        Assert.That(response.ErrorMessage, Is.EqualTo(string.Empty));
        Assert.That(response.Score, Is.GreaterThan(0));
    }
}