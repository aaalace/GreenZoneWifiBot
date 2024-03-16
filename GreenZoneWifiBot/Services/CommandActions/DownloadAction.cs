using GreenZoneWifiBot.Core;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace GreenZoneWifiBot.Services.CommandActions;

public static partial class Actions
{
    public static async Task DownloadAction(ITelegramBotClient botClient, CallbackQuery callback, CancellationToken cts)
    {
        if (callback.Message == null)
        {
            await ErrorAction(botClient, callback, cts);
            return;
        }
        
        var replyKeyboardMarkup = new InlineKeyboardMarkup(KeyBoards.DownloadKeyBoard);
        
        await botClient.SendTextMessageAsync(
            chatId: callback.Message.Chat.Id,
            text: "Choose format of file",
            replyMarkup: replyKeyboardMarkup,
            cancellationToken: cts);
    }
}