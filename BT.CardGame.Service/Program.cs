using BT.CardGame.Service.Services;
using NLog.Web;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ICardService, CardService>();

builder.Logging.ClearProviders();
builder.Logging.SetMinimumLevel(LogLevel.Trace);
builder.Host.UseNLog();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/play", (string cards, ICardService cardService) =>
    {
        var response = cardService.CalculateScore(cards);

        return string.IsNullOrEmpty(response.ErrorMessage) 
            ? Results.Ok(response.Score) 
            : Results.BadRequest(response.ErrorMessage);
    })
    .Produces<string>(400)
    .Produces<int>(200)
    .WithName("play")
    .WithOpenApi();

app.Run();