using Telegram.Bot.Types;

namespace GreenZoneWifiBot.Interfaces;

public interface IMessageService
{
    Task BotOnMessageReceived(Message message, CancellationToken cts);
}