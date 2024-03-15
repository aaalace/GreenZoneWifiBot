﻿using GreenZoneWifiBot.Utils.Logging;

namespace GreenZoneWifiBot.Services;

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
    
    public async Task BotOnCallbackQuieryReceived(CallbackQuery callbackQuery, CancellationToken cts)
    {
        _logger.LogInformation("{LogMessage}", LogManager.CreateCallbackLog(callbackQuery));
            
        var action = callbackQuery.Data switch
        {
            "/show" => CommandActions.ShowAction(_botClient, callbackQuery, cts),
            _ => ErrorAction(_botClient, callbackQuery, cts)
        };

        await action;
        
        await Task.CompletedTask;
        return;

        static async Task<Message> ErrorAction(ITelegramBotClient botClient, CallbackQuery callback, CancellationToken cts)
        {
            return await botClient.SendTextMessageAsync(
                chatId: callback.Message!.Chat.Id,
                text: "Sorry, I have nothing to tell you about this",
                cancellationToken: cts);
        }
    }
}