using GreenZoneWifiBot.Abstractions;
using Telegram.Bot;
using Telegram.Bot.Polling;

namespace GreenZoneWifiBot.Services;

public class ReceiverService : ReceiverServiceBase
{
    public ReceiverService(
        ITelegramBotClient botClient,
        UpdateHandler updateHandler,
        ILogger<ReceiverServiceBase> logger)
        : base(botClient, updateHandler, logger) {}
}