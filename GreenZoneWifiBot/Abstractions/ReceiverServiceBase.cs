using GreenZoneWifiBot.Services;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;

namespace GreenZoneWifiBot.Abstractions;

public abstract class ReceiverServiceBase
{
    private readonly ITelegramBotClient _botClient;
    private readonly UpdateHandler _updateHandler;
    private readonly ILogger<ReceiverServiceBase> _logger;

    internal ReceiverServiceBase(
        ITelegramBotClient botClient,
        UpdateHandler updateHandler,
        ILogger<ReceiverServiceBase> logger)
    {
        _botClient = botClient;
        _updateHandler = updateHandler;
        _logger = logger;
    }
    
    /// <summary>
    /// Configuring bot client on receive.
    /// </summary>v
    public async Task ReceiveAsync(CancellationToken cts)
    {
        var receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = new[] { UpdateType.Message, UpdateType.EditedMessage, UpdateType.CallbackQuery },
            ThrowPendingUpdates = true,
            Offset = 1
        };

        var me = await _botClient.GetMeAsync(cts);
        _logger.LogInformation("Start receiving updates for {BotName} | {DateTime}", me.Username ?? "GreenZoneWifiBot", DateTime.Now);
        
        await _botClient.ReceiveAsync(
            updateHandler: _updateHandler,
            receiverOptions: receiverOptions,
            cancellationToken: cts);
    }
}