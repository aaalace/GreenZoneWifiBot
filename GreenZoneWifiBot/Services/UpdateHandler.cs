using GreenZoneWifiBot.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace GreenZoneWifiBot.Services;

public class UpdateHandler : IUpdateHandler
{
    private readonly ILogger<UpdateHandler> _logger;
    private readonly IMessageService _messageService;
    
    public UpdateHandler(
        ILogger<UpdateHandler> logger,
        IMessageService messageService)
    {
        _logger = logger;
        _messageService = messageService;
    }

    public async Task HandleUpdateAsync(ITelegramBotClient _, Update update, CancellationToken cts)
    {
        var handlerByUpdateType = update switch
        {
            { Message: { } message } => _messageService.BotOnMessageReceived(message, cts),
            { EditedMessage: { } message } => _messageService.BotOnMessageReceived(message, cts),
            _ => Task.CompletedTask
        };

        await handlerByUpdateType;
    }
    
    public async Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cts)
    {
        var ErrorMessage = exception switch
        {
            ApiRequestException apiException => $"Telegram API Error: {apiException.Message}",
            _ => exception.ToString()
        };

        _logger.LogInformation("HandleError: {ErrorMessage}", ErrorMessage);
        
        if (exception is RequestException)
            await Task.Delay(TimeSpan.FromSeconds(2), cts);
    }
}