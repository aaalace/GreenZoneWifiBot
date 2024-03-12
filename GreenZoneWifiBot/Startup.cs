using dotenv.net;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;

namespace GreenZoneWifiBot;

public static class Startup
{
    public static void ConfigureEnvironment() => DotEnv.Load();
    
    public static ReceiverOptions ConfigureReceiverOptions()
    {
        var options = new ReceiverOptions
        {
            AllowedUpdates = Array.Empty<UpdateType>()
        };

        return options;
    }
}