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
    public int CalculateScore_ShouldCalcuateScore_WhenStandardCardCombinationsUsed(string cards)
    {
        return _cardService.CalculateScore(cards);
    }
    
    [TestCase("JK", ExpectedResult = 0)]
    [TestCase("JK,JK", ExpectedResult = 0)]
    [TestCase("2C,JK", ExpectedResult = 4)]
    [TestCase("JK,2C,JK", ExpectedResult = 8)]
    [TestCase("TC,TD,JK,TH,TS", ExpectedResult = 200)]
    [TestCase("TC,TD,JK,TH,TS,JK", ExpectedResult = 400)]
    public int CalculateScore_ShouldDoubleTheScore_WhenJokeIsUsed(string cards)
    {
        return _cardService.CalculateScore(cards);
    }
}