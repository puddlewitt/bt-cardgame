using System.Net;
using BT.CardGame.UI.Options;
using BT.CardGame.UI.Services;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;

namespace BT.CardGame.UI.Tests.Services;

public class CardGameScoreServiceTests
{
    private const string? ValidCards = "VALID_INPUT";
    private const string? InvalidCards = "INVALID_INPUT";
    private const string Score = "2";
    private const string InvalidCardsMessage = "INVALID_CARDS_MESSAGE";
    private const string SendAsyncMethodName = "SendAsync";
    private const string BaseUrl = "http://localhost";

    private ICardGameScoreService _cardGameScoreService;
    private CardGameScoreConfiguration _config;

    private CancellationTokenSource _cts;
    private HttpClient _httpClient;

    private Mock<IOptions<CardGameScoreConfiguration>> _mockConfig;
    private Mock<DelegatingHandler> _mockDelegatingHandler;
    private Mock<IUserInteractionService> _mockUserInteractionService;

    [TearDown]
    public void TearDown()
    {
        _httpClient.Dispose();
        _cts.Dispose();
    }

    [SetUp]
    public void SetUp()
    {
        _cts = new CancellationTokenSource();
        _config = new CardGameScoreConfiguration
        {
            BaseUrl = BaseUrl
        };

        _mockConfig = new Mock<IOptions<CardGameScoreConfiguration>>();
        _mockDelegatingHandler = new Mock<DelegatingHandler>();
        _mockUserInteractionService = new Mock<IUserInteractionService>();

        _mockConfig.Setup(m => m.Value)
            .Returns(_config);

        _mockDelegatingHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                SendAsyncMethodName,
                ItExpr.Is<HttpRequestMessage>(u =>
                    u.RequestUri != null && u.RequestUri.AbsoluteUri == $"{BaseUrl}/score?cards={ValidCards}"),
                ItExpr.IsAny<CancellationToken>()
            ).ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(Score)
            });

        _mockDelegatingHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                SendAsyncMethodName,
                ItExpr.Is<HttpRequestMessage>(u =>
                    u.RequestUri != null && u.RequestUri.AbsoluteUri == $"{BaseUrl}/score?cards={InvalidCards}"),
                ItExpr.IsAny<CancellationToken>()
            ).ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest,
                Content = new StringContent(InvalidCardsMessage)
            });

        _mockUserInteractionService.Setup(m => m.ReadLine())
            .Returns(ValidCards);

        _mockUserInteractionService.Setup(m => m.WriteLine(It.Is<string>(u => u == $"Score: {Score}"
                                                                              || u == InvalidCardsMessage)))
            .Callback(_cts.Cancel);

        _mockUserInteractionService.Setup(m => m.WriteLine(It.IsAny<Exception>()))
            .Callback(_cts.Cancel);

        _httpClient = new HttpClient(_mockDelegatingHandler.Object);

        _cardGameScoreService = new CardGameScoreService(_mockConfig.Object,
            _httpClient,
            _mockUserInteractionService.Object);
    }

    [Test]
    public async Task GoAsync_ShouldInteractWithUserAndPrintScoreToUser_WhenOKResponseReceived()
    {
        await _cardGameScoreService.GoAsync(_cts.Token);

        _mockUserInteractionService.Verify(
            m => m.WriteLine("Please enter your cards for a score calculation (or ctrl-c to exit). For example '2C'"),
            Times.Once);

        _mockUserInteractionService.Verify(m => m.ReadLine(),
            Times.Once);

        _mockUserInteractionService.Verify(m => m.WriteLine($"Score: {Score}"),
            Times.Once);

        _mockUserInteractionService.Verify(m => m.WriteLine(It.IsAny<Exception>()),
            Times.Never);
    }

    [Test]
    public async Task GoAsync_ShouldInteractWithUserAndPrintErrorToUser_WhenBadRequestReceived()
    {
        _mockUserInteractionService.Setup(m => m.ReadLine())
            .Returns(InvalidCards);

        await _cardGameScoreService.GoAsync(_cts.Token);

        _mockUserInteractionService.Verify(
            m => m.WriteLine("Please enter your cards for a score calculation (or ctrl-c to exit). For example '2C'"),
            Times.Once);

        _mockUserInteractionService.Verify(m => m.ReadLine(),
            Times.Once);

        _mockUserInteractionService.Verify(m => m.WriteLine(InvalidCardsMessage),
            Times.Once);

        _mockUserInteractionService.Verify(m => m.WriteLine(It.IsAny<Exception>()),
            Times.Never);
    }

    [Test]
    public async Task GoAsync_ShouldInteractWithUserAndPrintErrorToUser_WhenErrorThrown()
    {
        var ex = new Exception("OH_NO");

        _mockDelegatingHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                SendAsyncMethodName,
                ItExpr.Is<HttpRequestMessage>(u =>
                    u.RequestUri != null && u.RequestUri.AbsoluteUri == $"{BaseUrl}/score?cards={ValidCards}"),
                ItExpr.IsAny<CancellationToken>()
            ).Throws(() => ex);

        await _cardGameScoreService.GoAsync(_cts.Token);

        _mockUserInteractionService.Verify(
            m => m.WriteLine("Please enter your cards for a score calculation (or ctrl-c to exit). For example '2C'"),
            Times.Once);

        _mockUserInteractionService.Verify(m => m.ReadLine(),
            Times.Once);

        _mockUserInteractionService.Verify(m => m.WriteLine(It.IsAny<string>()),
            Times.Once);

        _mockUserInteractionService.Verify(m => m.WriteLine(ex),
            Times.Once);
    }

    [TestCase("")]
    [TestCase(" ")]
    public async Task GoAsync_ShouldInteractWithUserMultipleTimes_WhenInputIsEmpty(string inputFromUser)
    {
        var sequence = new MockSequence();

        _mockUserInteractionService.InSequence(sequence)
            .Setup(m => m.ReadLine())
            .Returns(inputFromUser);

        _mockUserInteractionService.InSequence(sequence)
            .Setup(m => m.ReadLine())
            .Returns(inputFromUser)
            .Callback(_cts.Cancel);

        await _cardGameScoreService.GoAsync(_cts.Token);

        _mockUserInteractionService.Verify(
            m => m.WriteLine("Please enter your cards for a score calculation (or ctrl-c to exit). For example '2C'"),
            Times.Exactly(2));
    }
}