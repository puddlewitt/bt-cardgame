using System.Net;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace BT.CardGame.Service.Tests.IntegrationTests;

public class ProgramTests : WebApplicationFactory<Program>
{
    private HttpClient _httpClient;

    [OneTimeSetUp]
    public void Init()
    {
        _httpClient = CreateClient();
    }

    [OneTimeTearDown]
    public void Cleanup()
    {
        _httpClient.Dispose();
    }

    [TestCase("2C", ExpectedResult = HttpStatusCode.OK)]
    [TestCase("2C,3C", ExpectedResult = HttpStatusCode.OK)]
    public async Task<HttpStatusCode> GetAsync_ShouldReturnOk_WhenValidCardsProvided(string cards)
    {
        var uri = BuildScoreRequest(cards);

        var response = await _httpClient.GetAsync(uri);

        return response.StatusCode;
    }

    [TestCase("2K", ExpectedResult = HttpStatusCode.BadRequest)]
    [TestCase("2C|3C", ExpectedResult = HttpStatusCode.BadRequest)]
    public async Task<HttpStatusCode> GetAsync_ShouldReturnBadRequest_WhenInValidCardsProvided(string cards)
    {
        var uri = BuildScoreRequest(cards);

        var response = await _httpClient.GetAsync(uri);

        return response.StatusCode;
    }

    private Uri BuildScoreRequest(string cards)
    {
        var queryBuilder = new QueryBuilder(new[]
        {
            new KeyValuePair<string, string>("cards", cards)
        });
        var builder = new UriBuilder
        {
            Query = queryBuilder.ToString(),
            Path = "score"
        };

        return builder.Uri;
    }
}