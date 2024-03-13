using GreenZoneWifiBot.Services;
using GreenZoneWifiBot.Interfaces;
using Telegram.Bot;

var host = Host.CreateDefaultBuilder()
    .ConfigureServices((context, services) =>
    {
        var config = context.Configuration;
        
        services.AddHttpClient("TelegramBotClient")
            .AddTypedClient<ITelegramBotClient>(_ =>
            {
                var token = config["TgBotToken"] ?? 
                            throw new Exception("Can not launch bot because bot token is not found");
                
                return new TelegramBotClient(new TelegramBotClientOptions(token));
            });

        services.AddTransient<IMessageService, MessageService>();

        services.AddScoped<UpdateHandler>();
        services.AddScoped<ReceiverService>();
        services.AddHostedService<PollingService>();
    })
    .Build();

await host.RunAsync();