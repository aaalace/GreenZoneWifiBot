using GreenZoneWifiBot.Services;
using Telegram.Bot;

var host = Host.CreateDefaultBuilder()
    .ConfigureServices((context, services) =>
    {
        var config = context.Configuration;
        
        services.AddHttpClient("TelegramBotClient")
            .AddTypedClient<ITelegramBotClient>(_ =>
            {
                var token = config["TgBotToken"] ?? throw new Exception("Token not found");
                var options = new TelegramBotClientOptions(token);
                return new TelegramBotClient(options);
            });

        services.AddScoped<UpdateHandler>();
        services.AddScoped<ReceiverService>();
        services.AddHostedService<PollingService>();
    })
    .Build();

await host.RunAsync();