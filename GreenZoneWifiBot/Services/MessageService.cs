using GreenZoneWifiBot.Interfaces;
using GreenZoneWifiBot.Utils;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

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
            MessageType.Sticker => StickerAction(_botClient, message, cts),
            MessageType.Document => DocumentAction(_botClient, message, cts),
            _ => ErrorAction(_botClient, message, cts)
        };
        
        await action;
        return;

        static async Task<Message> TextAction(ITelegramBotClient botClient, Message message, CancellationToken cts)
        {
            return await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: message.Text!,
                cancellationToken: cts);
        }
        
        static async Task<Message> StickerAction(ITelegramBotClient botClient, Message message, CancellationToken cts)
        {
            return await botClient.SendStickerAsync(
                chatId: message.Chat.Id,
                sticker: InputFile.FromFileId(message.Sticker!.FileId),
                cancellationToken: cts);
        }
        
        static async Task<Message> DocumentAction(ITelegramBotClient botClient, Message message, CancellationToken cts)
        {
            return await botClient.SendDocumentAsync(
                chatId: message.Chat.Id,
                document: InputFile.FromFileId(message.Document!.FileId),
                cancellationToken: cts);
        }
        
        static async Task<Message> ErrorAction(ITelegramBotClient botClient, Message message, CancellationToken cts)
        {
            return await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Sorry, I have nothing to tell you about this",
                cancellationToken: cts);
        }
    }
}