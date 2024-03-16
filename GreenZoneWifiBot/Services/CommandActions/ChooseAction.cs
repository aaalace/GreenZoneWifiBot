using GreenZoneWifiBot.Core;
using GreenZoneWifiBot.Utils;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace GreenZoneWifiBot.Services.CommandActions;

public static partial class Actions
{
    public static async Task ChooseAction(
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
        
        var replyKeyboardMarkup = new InlineKeyboardMarkup(KeyBoards.FieldsChoose);
        
        await botClient.SendTextMessageAsync(
            chatId: callback.Message.Chat.Id,
            text: "Pick up a field you want to make a choice by",
            replyMarkup: replyKeyboardMarkup,
            cancellationToken: cts);
    }
    
    public static async Task ChooseFieldAction(
        string field, ITelegramBotClient botClient, CallbackQuery callback, CancellationToken cts)
    {
        if (callback.Message == null)
        {
            await ErrorCallbackAction(botClient, callback, cts);
            return;
        }
        
        var dirPath = PathGetter.Get("tmpChoice");
        var chatId = callback.Message.Chat.Id;
        var fullPath = Path.Combine(dirPath, chatId.ToString());
        
        await System.IO.File.WriteAllTextAsync(fullPath, field, cts);

        await botClient.SendTextMessageAsync(
            chatId: callback.Message.Chat.Id,
            text: $"Enter value for {field.Replace("_choice", "")} to choose by",
            cancellationToken: cts);
    }
}