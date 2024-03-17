// https://web.telegram.org/k/#@GreenZoneWifiBot

using GreenZoneWifiBot.Interfaces;
using GreenZoneWifiBot.Services;
using GreenZoneWifiBot.Services.UpdateActions;
using GreenZoneWifiBot.Utils;
using GreenZoneWifiBot.Utils.Logging;
using Telegram.Bot;

if (!Directory.Exists("uploads")) Directory.CreateDirectory("uploads");
if (!Directory.Exists("var")) Directory.CreateDirectory("var");
if (!Directory.Exists("tmpChoice")) Directory.CreateDirectory("tmpChoice");

// Creating host builder.
var builder = Host.CreateDefaultBuilder();

// App configuration.
builder.ConfigureAppConfiguration(configBuilder =>
    configBuilder.AddJsonFile("appsettings.Secrets.json", optional: true, reloadOnChange: true));

// Logging configuration.
builder.ConfigureLogging(loggingBuilder =>
{
    var loggerPath = PathGetter.Get("var");
    loggingBuilder.AddProvider(new FileLoggerProvider(loggerPath));
});
    
// Services configuration.
builder.ConfigureServices((context, services) =>
{
    // Getting tg bot token.
    var token = context.Configuration["GreenZoneBotToken"] ??
                throw new Exception("Can not launch bot because token was not found");

    // Adding tg bot as HttpClient.
    services.AddHttpClient("TelegramBotClient")
        .AddTypedClient<ITelegramBotClient>(_ => new TelegramBotClient(new TelegramBotClientOptions(token)));
    
    // Adding services.
    services.AddTransient<IMessageService, MessageService>();
    services.AddTransient<ICallbackQuieryService, CallbackQuieryService>();
    services.AddScoped<UpdateHandler>();
    services.AddScoped<ReceiverService>();
    services.AddHostedService<PollingService>();
});

// Building host.
using var host = builder.Build();

// Running host.
await host.RunAsync();