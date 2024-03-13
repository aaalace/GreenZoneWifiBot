using GreenZoneWifiBot.Services;
using GreenZoneWifiBot.Interfaces;
using Telegram.Bot;

var host = Host.CreateDefaultBuilder()
    .ConfigureAppConfiguration((context, config) =>
    {
        var env = context.HostingEnvironment;
        config.AddJsonFile("appsettings.Secrets.json", optional: true, reloadOnChange: true);
    })
    .ConfigureServices((context, services) =>
    {
        services.AddHttpClient("TelegramBotClient")
            .AddTypedClient<ITelegramBotClient>(_ =>
            {
                var token = context.Configuration["GreenZoneBotToken"] ?? 
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