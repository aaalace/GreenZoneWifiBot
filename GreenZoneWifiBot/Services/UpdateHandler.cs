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
    private readonly ICallbackQuieryService _callbackQuieryService;
    
    public UpdateHandler(
        ILogger<UpdateHandler> logger,
        IMessageService messageService,
        ICallbackQuieryService callbackQuieryService)
    {
        _logger = logger;
        _messageService = messageService;
        _callbackQuieryService = callbackQuieryService;
    }

    public async Task HandleUpdateAsync(ITelegramBotClient _, Update update, CancellationToken cts)
    {
        var handlerByUpdateRequestType = update switch
        {
            { Message: { } message } => _messageService.BotOnMessageReceived(message, cts),
            { EditedMessage: { } message } => _messageService.BotOnMessageReceived(message, cts),
            { CallbackQuery: {} callbackQuery } => _callbackQuieryService.BotOnCallbackQuieryReceived(callbackQuery, cts),
            _ => Task.CompletedTask
        };
        
        await handlerByUpdateRequestType;
    }
    
    public async Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cts)
    {
        var ErrorMessage = exception switch
        {
            ApiRequestException apiException => $"Telegram API Error: {apiException.Message}",
            _ => exception.ToString()
        };

        _logger.LogInformation("Error handled: {ErrorMessage}", ErrorMessage);
        
        if (exception is RequestException)
            await Task.Delay(TimeSpan.FromSeconds(2), cts);
    }
}