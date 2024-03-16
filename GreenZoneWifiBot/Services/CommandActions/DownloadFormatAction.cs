using GreenZoneWifiBot.Core;
using GreenZoneWifiBot.Utils;
using Lib;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace GreenZoneWifiBot.Services.CommandActions;

public static partial class Actions
{
    public static async Task DownloadFormatAction(ITelegramBotClient botClient, CallbackQuery callback, CancellationToken cts)
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
        
        var stream = System.IO.File.OpenRead(fullPath);
        var name = callback.Message.Chat.Username ?? chatId.ToString();
        try
        {
            if (callback.Data == "/downloadCsv")
            {
                name += ".csv";

                var json = new JsonProcessing();
                var collection = await json.Read(stream);
                stream.Close();
                var csv = new CsvProcessing();
                var tmpPath = fullPath + "_tmp";
                var csvStream = csv.Write(collection, tmpPath);
                csvStream.Close();
                var csvTmpStream = System.IO.File.OpenRead(tmpPath);
                
                await botClient.SendDocumentAsync(
                    chatId: chatId,
                    document: InputFile.FromStream(stream: csvTmpStream, fileName: name),
                    caption: "Here is your file",
                    cancellationToken: cts);
                
                csvTmpStream.Close();
                System.IO.File.Delete(tmpPath);
            }
            else
            {
                name += ".json";
                await botClient.SendDocumentAsync(
                    chatId: chatId,
                    document: InputFile.FromStream(stream: stream, fileName: name),
                    caption: "Here is your file",
                    cancellationToken: cts);
        
                stream.Close();
            }
            
            await botClient.SendTextMessageAsync(
                chatId: callback.Message!.Chat.Id,
                text: "Now you can upload new file, or continue working with current one",
                replyMarkup: new InlineKeyboardMarkup(KeyBoards.FileWorkKeyBoard),
                cancellationToken: cts);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            await ErrorAction(botClient, callback, cts, "Error while downloading file, please try later");
        }
    }
}