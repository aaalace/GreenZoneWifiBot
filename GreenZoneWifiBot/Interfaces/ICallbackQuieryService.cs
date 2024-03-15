using Telegram.Bot.Types;

namespace GreenZoneWifiBot.Interfaces;

public interface ICallbackQuieryService
{ 
    Task BotOnCallbackQuieryReceived(CallbackQuery callbackQuery, CancellationToken cts);
}