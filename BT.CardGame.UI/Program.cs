using BT.CardGame.UI.Options;
using BT.CardGame.UI.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = Host.CreateApplicationBuilder(args);

builder.Logging.SetMinimumLevel(LogLevel.Error);

builder.Services.AddOptions<CardGameScoreConfiguration>()
    .BindConfiguration(string.Empty);
builder.Services.AddHttpClient();
builder.Services.AddScoped<IUserInteractionService, ConsoleService>();
builder.Services.AddScoped<ICardGameScoreService, CardGameScoreService>();

using var host = builder.Build();

var scoreService = host.Services.GetService<ICardGameScoreService>();
var cts = new CancellationTokenSource();

if (scoreService == null)
{
    Console.WriteLine("Unable to start application. Cannot find service.");
}
else
{
    await scoreService.GoAsync(cts.Token);
}