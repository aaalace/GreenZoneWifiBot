using GreenZoneWifiBot.Abstractions;
using Telegram.Bot;

namespace GreenZoneWifiBot.Services;

public class ReceiverService : ReceiverServiceBase
{
    public ReceiverService(
        ITelegramBotClient botClient,
        UpdateHandler updateHandler,
        ILogger<ReceiverServiceBase> logger)
        : base(botClient, updateHandler, logger) {}
}