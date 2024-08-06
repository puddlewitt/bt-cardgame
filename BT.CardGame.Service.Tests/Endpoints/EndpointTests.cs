using BT.CardGame.Service.Endpoints;
using BT.CardGame.Service.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;
using Moq;

namespace BT.CardGame.Service.Tests.Endpoints;

public class EndpointTests
{
    private const string OkCards = "2C";
    private const string FailCards = "2C|3C";
    private const string ErrorMessage = "AN_ERROR_MESSAGE";
    private const int Score = 1;

    private Mock<ICardService> _mockCardService;
    
    [SetUp]
    public void SetUp()
    {
        _mockCardService = new Mock<ICardService>();

        _mockCardService.Setup(m => m.CalculateScore(OkCards))
            .Returns((Score, string.Empty));

        _mockCardService.Setup(m => m.CalculateScore(FailCards))
            .Returns((0, ErrorMessage));
    }

    [Test]
    public void Handler_ShouldReturnOkWithScore_WhenValidCardsSubmitted()
    {
        var result = PlayEndpoints.PlayHandler(OkCards, _mockCardService.Object);

        Assert.That(result, Is.TypeOf<Ok<int>>());
        
        var okResult = (Ok<int>)result;
        
        Assert.That(okResult.Value, Is.EqualTo(Score));
    }
    
    [Test]
    public void Handler_ShouldReturnBadRequestWithErrorMessage_WhenInvalidCardsSubmitted()
    {
        var result = PlayEndpoints.PlayHandler(FailCards, _mockCardService.Object);
     
        Assert.That(result, Is.TypeOf<BadRequest<string>>());
        
        var badRequestResult = (BadRequest<string>)result;

        Assert.That(badRequestResult.Value, Is.EqualTo(ErrorMessage));
    }
}