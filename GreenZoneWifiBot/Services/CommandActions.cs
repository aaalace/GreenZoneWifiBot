using GreenZoneWifiBot.Core;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace GreenZoneWifiBot.Services;

public static class CommandActions
{
    // start command
    public static async Task StartAction(ITelegramBotClient botClient, Message message, CancellationToken cts)
    {
        const string homikStickerId = "CAACAgIAAxkBAAIBmGXzTJdKmv_LhR8AAb7Kq-RV7az4MQACCz8AAk3owEoYLT-AVGnB9TQE";
        var replyKeyboardMarkup = new ReplyKeyboardMarkup(KeyBoards.IndexKeyBoard) { ResizeKeyboard = true };
        
        await botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: "Hi. I am GreenZone Wi-Fi Assistant, send me your json or csv file",
            replyMarkup: replyKeyboardMarkup,
            cancellationToken: cts);

        await botClient.SendStickerAsync(
            chatId: message.Chat.Id,
            sticker: InputFile.FromFileId(homikStickerId),
            cancellationToken: cts);
    }
    
    // show command
    public static async Task ShowAction(ITelegramBotClient botClient, CallbackQuery callback, CancellationToken cts)
    {
        await botClient.SendTextMessageAsync(
            chatId: callback.Message!.Chat.Id,
            text: "```json\nHere will be json or csv```",
            parseMode: ParseMode.MarkdownV2, 
            cancellationToken: cts);
    } 
}