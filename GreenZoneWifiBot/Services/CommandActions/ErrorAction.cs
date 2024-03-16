using Telegram.Bot;
using Telegram.Bot.Types;

namespace GreenZoneWifiBot.Services.CommandActions;

public static partial class Actions
{
    private static async Task ErrorAction(ITelegramBotClient botClient, CallbackQuery callback, 
        CancellationToken cts, string text = "Sorry, I have nothing to tell you about this")
    {
        await botClient.SendTextMessageAsync(
            chatId: callback.Message!.Chat.Id,
            text: text,
            cancellationToken: cts);
    }
}