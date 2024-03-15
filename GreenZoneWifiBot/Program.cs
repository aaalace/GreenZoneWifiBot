using GreenZoneWifiBot.Services;
using GreenZoneWifiBot.Interfaces;
using GreenZoneWifiBot.Utils.Logging;
using Telegram.Bot;

// Creating host builder.
var builder = Host.CreateDefaultBuilder();

// App configuration.
builder.ConfigureAppConfiguration(configBuilder =>
    configBuilder.AddJsonFile("appsettings.Secrets.json", optional: true, reloadOnChange: true));

// Logging configuration.
builder.ConfigureLogging(loggingBuilder =>
{
    var curenntDir = new DirectoryInfo(Directory.GetCurrentDirectory());
    var varSaveDirPath = curenntDir.Parent ?? curenntDir;
    loggingBuilder.AddProvider(new FileLoggerProvider(Path.Combine(varSaveDirPath.FullName, "var")));
});
    
// Services configuration.
builder.ConfigureServices((context, services) =>
{
    // Getting tg bot token.
    var token = context.Configuration["GreenZoneBotToken"] ??
                throw new Exception("Can not launch bot because bot token is not found");

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