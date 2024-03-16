using GreenZoneWifiBot.Core;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace GreenZoneWifiBot.Services.CommandActions;

public static partial class Actions
{
    public static async Task StartAction(
        ITelegramBotClient botClient, Message message, CancellationToken cts)
    {
        const string homikStickerId = "CAACAgIAAxkBAAIBmGXzTJdKmv_LhR8AAb7Kq-RV7az4MQACCz8AAk3owEoYLT-AVGnB9TQE";
        
        await botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: "Hi. I am GreenZone Wi-Fi Assistant, send me your json or csv file",
            cancellationToken: cts);

        await botClient.SendStickerAsync(
            chatId: message.Chat.Id,
            sticker: InputFile.FromFileId(homikStickerId),
            cancellationToken: cts);
    }
}