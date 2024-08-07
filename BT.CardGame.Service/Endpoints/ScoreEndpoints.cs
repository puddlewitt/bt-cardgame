using BT.CardGame.Service.Services;

namespace BT.CardGame.Service.Endpoints;

public static class ScoreEndpoints
{
    public static RouteGroupBuilder MapScoreApi(this RouteGroupBuilder group)
    {
        group.MapGet("/score", ScoreHandler)
            .Produces<string>(400)
            .Produces<int>()
            .WithName("score")
            .WithOpenApi();

        return group;
    }

    public static IResult ScoreHandler(string cards, ICardService cardService)
    {
        var response = cardService.CalculateScore(cards);

        return string.IsNullOrEmpty(response.ErrorMessage)
            ? Results.Ok(response.Score)
            : Results.BadRequest(response.ErrorMessage);
    }
}