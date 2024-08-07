using System.Net;
using System.Web;
using BT.CardGame.UI.Options;
using Microsoft.Extensions.Options;

namespace BT.CardGame.UI.Services;

public class CardGameScoreService : ICardGameScoreService
{
    private readonly IOptions<CardGameScoreConfiguration> _config;
    private readonly HttpClient _httpClient;
    private readonly IUserInteractionService _userInteractionService;

    public CardGameScoreService(IOptions<CardGameScoreConfiguration> config,
        HttpClient httpClient,
        IUserInteractionService userInteractionService)
    {
        _config = config;
        _httpClient = httpClient;
        _userInteractionService = userInteractionService;
    }

    public async Task GoAsync(CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            _userInteractionService.WriteLine(
                "Please enter your cards for a score calculation (or ctrl-c to exit). For example '2C'");

            var input = _userInteractionService.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
            {
                continue;
            }

            try
            {
                var query = HttpUtility.ParseQueryString(string.Empty);

                query["cards"] = input;

                var queryString = query.ToString();
                var uri = new Uri($"{_config.Value.BaseUrl}/score?{queryString}", UriKind.Absolute);

                var response = await _httpClient.GetAsync(uri, ct);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    _userInteractionService.WriteLine($"Score: {await response.Content.ReadAsStringAsync(ct)}");
                }
                else
                {
                    _userInteractionService.WriteLine(await response.Content.ReadAsStringAsync(ct));
                }
            }
            catch (Exception ex)
            {
                _userInteractionService.WriteLine(ex);
            }
        }
    }
}