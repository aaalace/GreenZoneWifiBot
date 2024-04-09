using Telegram.Bot;
using Telegram.Bot.Types;

namespace GreenZoneWifiBot.Services.CommandActions;

public static partial class Actions
{
    /// <summary>
    /// Error action for "callback" requests. 
    /// </summary>
    public static async Task ErrorCallbackAction(ITelegramBotClient botClient, CallbackQuery callback, 
        CancellationToken cts, string text = "Sorry, I have nothing to tell you about this")
    {
        await botClient.SendTextMessageAsync(
            chatId: callback.Message!.Chat.Id,
            text: text,
            cancellationToken: cts);
    }
    
    /// <summary>
    /// Error action for "message" requests. 
    /// </summary>
    public static async Task ErrorMessageAction(ITelegramBotClient botClient, Message message, 
        CancellationToken cts, string text = "Sorry, I have nothing to tell you about this")
    {
        await botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: text,
            cancellationToken: cts);
    }
}