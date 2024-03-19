using GreenZoneWifiBot.Core;
using GreenZoneWifiBot.Utils;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace GreenZoneWifiBot.Services.CommandActions;

public static partial class Actions
{
    /// <summary>
    /// Calls when "download" pressed. Gives user a choice: csv or json format.
    /// </summary>
    public static async Task DownloadAction(
        ITelegramBotClient botClient, CallbackQuery callback, CancellationToken cts)
    {
        if (callback.Message == null)
        {
            await ErrorCallbackAction(botClient, callback, cts);
            return;
        }
        
        var dirPath = PathGetter.Get("uploads");
        var chatId = callback.Message.Chat.Id;
        var fullPath = Path.Combine(dirPath, chatId.ToString());

        if (!System.IO.File.Exists(fullPath))
        {
            await ErrorCallbackAction(botClient, callback, cts, "File was not uploaded yet, maybe it had a wrong format");
            return;
        }
        
        var replyKeyboardMarkup = new InlineKeyboardMarkup(KeyBoards.DownloadKeyBoard);
        
        await botClient.SendTextMessageAsync(
            chatId: callback.Message.Chat.Id,
            text: "Choose format of file you want to download",
            replyMarkup: replyKeyboardMarkup,
            cancellationToken: cts);
    }
}