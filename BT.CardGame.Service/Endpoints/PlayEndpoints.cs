using BT.CardGame.Service.Services;

namespace BT.CardGame.Service.Endpoints;

public static class PlayEndpoints
{
    public static RouteGroupBuilder MapPlayApi(this RouteGroupBuilder group)
    {
        group.MapGet("/play", PlayHandler)
            .Produces<string>(400)
            .Produces<int>()
            .WithName("play")
            .WithOpenApi();

        return group;
    }

    public static IResult PlayHandler(string cards, ICardService cardService)
    {
        var response = cardService.CalculateScore(cards);

        return string.IsNullOrEmpty(response.ErrorMessage)
            ? Results.Ok(response.Score)
            : Results.BadRequest(response.ErrorMessage);
    }
}