using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace GreenZoneWifiBot.Services;

public class UpdateHandler : IUpdateHandler
{
    private readonly ITelegramBotClient _botClient;
    private readonly ILogger<UpdateHandler> _logger;
    
    public UpdateHandler(ITelegramBotClient botClient, ILogger<UpdateHandler> logger)
    {
        _botClient = botClient;
        _logger = logger;
    }

    public async Task HandleUpdateAsync(ITelegramBotClient _, Update update, CancellationToken cancellationToken)
    {
        var handler = update switch
        {
            { Message: { } message } => BotOnMessageReceived(message, cancellationToken),
            _ => UnknownUpdateHandlerAsync(update)
        };

        await handler;
    }

    private async Task BotOnMessageReceived(Message message, CancellationToken cts)
    {
        _logger.LogInformation("Receive message type: {MessageType}", message.Type);
        
        if (message.Text is null) return;

        var action = message.Text.Split(' ')[0] switch
        {
            "/smth" => Method(_botClient, message, cts),
            _ => MethodError(_botClient, message, cts)
        };
        
        await action;
        return;

        static async Task<Message> Method(ITelegramBotClient botClient, Message message, CancellationToken cts)
        {
            return await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Choose",
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
    
    private Task UnknownUpdateHandlerAsync(Update update)
    {
        _logger.LogInformation("Unknown update type: {UpdateType}", update.Type);
        return Task.CompletedTask;
    }
    
    public async Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        var ErrorMessage = exception switch
        {
            ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };

        _logger.LogInformation("HandleError: {ErrorMessage}", ErrorMessage);
        
        if (exception is RequestException)
            await Task.Delay(TimeSpan.FromSeconds(2), cancellationToken);
    }
}