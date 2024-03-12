using Telegram.Bot;
using dotenv.net;

namespace GreenZoneWifiBot;

public static class Solution
{
    public static async Task Main()
    {
        ConfigureApplication();

        var tgBotToken = Environment.GetEnvironmentVariable("TgBotToken") ?? "";
        var botClient = new TelegramBotClient(tgBotToken);
        var me = await botClient.GetMeAsync();
        Console.WriteLine($"Hello, World! I am user {me.Id} and my name is {me.FirstName}.");
    }

    private static void ConfigureApplication()
    {
        DotEnv.Load();
    }
}