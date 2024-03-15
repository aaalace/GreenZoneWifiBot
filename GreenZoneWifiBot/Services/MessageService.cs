using GreenZoneWifiBot.Core;
using GreenZoneWifiBot.Interfaces;
using GreenZoneWifiBot.Utils.Logging;
using Lib;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace GreenZoneWifiBot.Services;

public class MessageService : IMessageService
{
    private readonly ITelegramBotClient _botClient;
    private readonly ILogger<MessageService> _logger;

    public MessageService(ITelegramBotClient botClient, ILogger<MessageService> logger)
    {
        _logger = logger;
        _botClient = botClient;
    }
    
    public async Task BotOnMessageReceived(Message message, CancellationToken cts)
    {
        _logger.LogInformation("{LogMessage}",LogManager.CreateMessageLog(message));
        
        var action = message.Type switch
        {
            MessageType.Text => TextAction(_botClient, message, cts),
            MessageType.Document => DocumentAction(_botClient, message, cts),
            _ => ErrorAction(_botClient, message, cts)
        };
        
        await action;
        return;

        static async Task TextAction(ITelegramBotClient botClient, Message message, CancellationToken cts)
        {
            var action = message.Text switch
            {
               "/start" => CommandActions.StartAction(botClient, message, cts),
               _ => ErrorAction(botClient, message, cts)
            };

            await action;
        }
        
        static async Task DocumentAction(ITelegramBotClient botClient, Message message, CancellationToken cts)
        {
            var document = message.Document!;
            var chatId = message.Chat.Id.ToString();
            
            var file = await botClient.GetFileAsync(document.FileId, cts);
            if (file.FilePath == null)
            {
                await ErrorAction(botClient, message, cts, "Error in uploading file, try again later");
                return;
            }

            try
            {
                var curenntDir = new DirectoryInfo(Directory.GetCurrentDirectory());
                var varParrentPath = curenntDir.Parent ?? curenntDir;
                var savePath = Path.Combine(varParrentPath.FullName, "uploads");
                var localPath = Path.Combine(savePath, chatId);
                
                var stream = new FileStream(localPath, FileMode.Create);
                await botClient.DownloadFileAsync(file.FilePath, stream, cts);
                stream.Close();
                
                var csvStream = new FileStream(localPath, FileMode.Open);
                var csv = new CsvProcessing();
                await csv.Read(csvStream);
                csvStream.Close();
                if (!csv.State)
                {
                    var jsonStream = new FileStream(localPath, FileMode.Open);
                    var json = new JsonProcessing();
                    await json.Read(jsonStream);
                    jsonStream.Close();
                    if (!json.State)
                    {
                        System.IO.File.Delete(localPath);
                    
                        await botClient.SendTextMessageAsync(
                            chatId: chatId,
                            text: "Wrong data format in file",
                            cancellationToken: cts);
                        return;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                await ErrorAction(botClient, message, cts, "Error in uploading file, try again later");
                return;
            }
            
            await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: $"<b>{document.FileName}</b> was successfully uploaded, what do you want to do next?",
                parseMode: ParseMode.Html,
                replyMarkup: new InlineKeyboardMarkup(KeyBoards.FileWorkKeyBoard),
                cancellationToken: cts);
        }
        
        static async Task ErrorAction(ITelegramBotClient botClient, Message message, CancellationToken cts, 
            string text = "Sorry, I have nothing to tell you about this")
        {
            await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: text,
                cancellationToken: cts);
        }
    }
}