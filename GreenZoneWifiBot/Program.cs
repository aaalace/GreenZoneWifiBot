using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace GreenZoneWifiBot;

public static class Solution
{
    private static ITelegramBotClient? _botClient;
    private static ReceiverOptions? _receiverOptions;
    
    public static async Task Main()
    {
        Startup.ConfigureEnvironment();

        // Environment variable TgBotToken
        var tgBotToken = Environment.GetEnvironmentVariable("TgBotToken") ?? 
                         throw new Exception("Can not find token");

        _botClient = new TelegramBotClient(tgBotToken);

        _receiverOptions = Startup.ConfigureReceiverOptions();
        
        using CancellationTokenSource cts = new();

        _botClient.StartReceiving(
            updateHandler: HandleUpdateAsync,
            pollingErrorHandler: HandlePollingErrorAsync,
            receiverOptions: _receiverOptions,
            cancellationToken: cts.Token
        );
        
        var bot = await _botClient.GetMeAsync(cts.Token);
        // TODO: Microsoft Extensions Logging
        Console.WriteLine($"{bot.IsBot} <> {bot.FirstName} started");
        
        await Task.Delay(-1, cts.Token);
    }
    
    private static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if (update.Message is not { } message) return;
        if (message.Text is not { } messageText) return;
        
        var chatId = message.Chat.Id;
        var user = message.From;

        // TODO: Microsoft Extensions Logging (also make a message sample)
        Console.WriteLine($"Received a '{message.Text}' message in chat {chatId} from {user}, date: {message.Date}.");
        
        // Echo
        await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: "You said:\n" + messageText,
            cancellationToken: cancellationToken
            );
    } 

    private static Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        var ErrorMessage = exception switch
        {
            ApiRequestException apiRequestException
                => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };

        // TODO: Microsoft Extensions Logging 
        Console.WriteLine(ErrorMessage);
        
        return Task.CompletedTask;
    }
}