using GreenZoneWifiBot.Core;
using GreenZoneWifiBot.Services.CommandActions;
using GreenZoneWifiBot.Utils.Logging;

namespace GreenZoneWifiBot.Services.UpdateActions;

using Interfaces;
using Telegram.Bot;
using Telegram.Bot.Types;

public class CallbackQuieryService : ICallbackQuieryService
{
    private readonly ITelegramBotClient _botClient;
    private readonly ILogger<MessageService> _logger;

    public CallbackQuieryService(ITelegramBotClient botClient, ILogger<MessageService> logger)
    {
        _logger = logger;
        _botClient = botClient;
    }
    
    /// <summary>
    /// Handles callbacks. Calls different actions in different cases.
    /// </summary>
    public async Task BotOnCallbackQuieryReceived(CallbackQuery callbackQuery, CancellationToken cts)
    {
        _logger.LogInformation("{LogMessage}", LogManager.CreateCallbackLog(callbackQuery));
        
        var action = callbackQuery.Data switch
        {
            "/show" => 
                Actions.ShowAction(_botClient, callbackQuery, cts),
            "/download" => 
                Actions.DownloadAction(_botClient, callbackQuery, cts),
            "/downloadJson" or "/downloadCsv" => 
                Actions.DownloadFormatAction(_botClient, callbackQuery, cts),
            "/sort" => 
                Actions.SortAction(_botClient, callbackQuery, cts),
            "/choose" => 
                Actions.ChooseAction(_botClient, callbackQuery, cts),
            { } field when Fields.values.Contains(field) => 
                Actions.SortFieldAction(field, _botClient, callbackQuery, cts),
            { } field when Fields.valuesChoice.Contains(field) => 
                Actions.ChooseFieldAction(field, _botClient, callbackQuery, cts),
            _ => Actions.ErrorCallbackAction(_botClient, callbackQuery, cts)
        };

        await action;
        
        await Task.CompletedTask;
    }
}