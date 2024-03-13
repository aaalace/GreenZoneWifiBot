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
        _logger.LogInformation("<Recieve update> {Log}", LogManager.LogMessage(message));
        
        var action = message.Type switch
        {
            MessageType.Text => Method(_botClient, message, cts),
            MessageType.Sticker => MethodSticker(_botClient, message, cts),
            _ => MethodError(_botClient, message, cts)
        };
        
        await action;
        return;

        static async Task<Message> Method(ITelegramBotClient botClient, Message message, CancellationToken cts)
        {
            return await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: message.Text!,
                cancellationToken: cts);
        }
        
        static async Task<Message> MethodSticker(ITelegramBotClient botClient, Message message, CancellationToken cts)
        {
            return await botClient.SendStickerAsync(
                chatId: message.Chat.Id,
                sticker: InputFile.FromFileId(message.Sticker!.FileId),
                cancellationToken: cts);
        }
        
        static async Task<Message> MethodError(ITelegramBotClient botClient, Message message, CancellationToken cts)
        {
            return await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Error",
                cancellationToken: cts);
        }
    }
}