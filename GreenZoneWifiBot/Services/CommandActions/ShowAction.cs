using GreenZoneWifiBot.Core;
using GreenZoneWifiBot.Utils;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace GreenZoneWifiBot.Services.CommandActions;

public static partial class Actions
{
    // show command
    public static async Task ShowAction(ITelegramBotClient botClient, CallbackQuery callback, CancellationToken cts)
    {
        if (callback.Message == null)
        {
            await ErrorAction(botClient, callback, cts);
            return;
        }

        var dirPath = PathGetter.Get("uploads");
        var chatId = callback.Message.Chat.Id;
        var fullPath = Path.Combine(dirPath, chatId.ToString());

        if (!System.IO.File.Exists(fullPath))
        {
            await ErrorAction(botClient, callback, cts, "File was not uploaded yet, maybe it had a wrong format");
            return;
        }
        
        string data = await System.IO.File.ReadAllTextAsync(fullPath, cts);

        string sendData = new string(data);
        if (sendData.Length > 4096)
            sendData = sendData[..4096];
        
        await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: $"```json\n{sendData}```",
            parseMode: ParseMode.MarkdownV2,
            replyMarkup: new InlineKeyboardMarkup(KeyBoards.AfterShowingDataKeyBoard),
            cancellationToken: cts);

        if (data.Length > 4096)
        {
            await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "Sorry, data is too long to be shown fully\nIf you want to watch it please download",
                cancellationToken: cts);
        }
    } 
}