using GreenZoneWifiBot.Core;
using GreenZoneWifiBot.Interfaces;
using GreenZoneWifiBot.Services.CommandActions;
using GreenZoneWifiBot.Utils;
using GreenZoneWifiBot.Utils.Logging;
using Lib;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace GreenZoneWifiBot.Services.UpdateActions;

public class MessageService : IMessageService
{
    private readonly ITelegramBotClient _botClient;
    private readonly ILogger<MessageService> _logger;

    public MessageService(ITelegramBotClient botClient, ILogger<MessageService> logger)
    {
        _logger = logger;
        _botClient = botClient;
    }
    
    /// <summary>
    /// Handles messages. Calls different actions in different cases.
    /// </summary>
    public async Task BotOnMessageReceived(Message message, CancellationToken cts)
    {
        _logger.LogInformation("{LogMessage}",LogManager.CreateMessageLog(message));
        
        var action = message.Type switch
        {
            MessageType.Text => TextAction(_botClient, message, cts),
            MessageType.Document => DocumentAction(_botClient, message, cts),
            _ => Actions.ErrorMessageAction(_botClient, message, cts)
        };
        
        await action;
        return;

        // If message - text.
        static async Task TextAction(ITelegramBotClient botClient, Message message, CancellationToken cts)
        {
            var action = message.Text switch
            {
               "/start" => Actions.StartAction(botClient, message, cts),
               _ => Actions.PlainTextAction(botClient, message, cts)
            };

            await action;
        }
        
        // If message - document.
        static async Task DocumentAction(ITelegramBotClient botClient, Message message, CancellationToken cts)
        {
            var document = message.Document!;
            var chatId = message.Chat.Id.ToString();
            
            var file = await botClient.GetFileAsync(document.FileId, cts);
            if (file.FilePath == null)
            {
                await Actions.ErrorMessageAction(botClient, message, cts, "Error in uploading file, try again later");
                return;
            }

            try
            {
                // Getting full path to user's file.
                var savePath = PathGetter.Get("uploads");
                var localPath = Path.Combine(savePath, chatId);
                
                // Write to file.
                var stream = new FileStream(localPath, FileMode.Create);
                await botClient.DownloadFileAsync(file.FilePath, stream, cts);
                stream.Close();
                
                // Check if json format.
                var jsonStream = new FileStream(localPath, FileMode.Open);
                var json = new JsonProcessing();
                await json.Read(jsonStream);
                jsonStream.Close();
                
                if (!json.State)
                {
                    // Check for csv format if not json format.
                    var csvStream = new FileStream(localPath, FileMode.Open);
                    var csv = new CsvProcessing();
                    var collection = csv.Read(csvStream);
                    csvStream.Close();
                    
                    // Delete created file if neither json nor csv format.
                    if (!csv.State)
                    {
                        System.IO.File.Delete(localPath);

                        await botClient.SendTextMessageAsync(
                            chatId: chatId,
                            text: "Wrong data format in file, send a new one",
                            cancellationToken: cts);
                        return;
                    }

                    // Write to file as json if csv format
                    var jsonUpd = new JsonProcessing(localPath);
                    var jsonUpdStream = await jsonUpd.Write(collection);
                    jsonUpdStream.Close();
                }
            }
            catch (Exception)
            {
                await Actions.ErrorMessageAction(botClient, message, cts, "Error in uploading file, try again later");
                return;
            }
            
            await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: $"<b>{document.FileName}</b> was successfully uploaded, what do you want to do next?",
                parseMode: ParseMode.Html,
                replyMarkup: new InlineKeyboardMarkup(KeyBoards.FileWorkKeyBoard),
                cancellationToken: cts);
        }
    }
}