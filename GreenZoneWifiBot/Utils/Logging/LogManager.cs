using System.Text;
using Telegram.Bot.Types;

namespace GreenZoneWifiBot.Utils.Logging;

public static class LogManager
{
    public static string CreateMessageLog(Message message)
    {
        var sb = new StringBuilder();
        sb.Append($"> MessageId: {message.MessageId}");
        sb.Append($"|ChatId: {message.Chat.Id}");
        sb.Append($"|MessageType: {message.Type}");
        sb.Append($"|DateTime: {message.Date}");
        if (message.From != null) sb.Append($"|UserFrom: {message.From}");
        
        return sb.ToString();
    }
    
    public static string CreateCallbackLog(CallbackQuery callback)
    {
        var sb = new StringBuilder();
        sb.Append($"> Callback data: {callback.Data}");
        sb.Append($"|MessageId: {callback.Message!.MessageId}");
        sb.Append($"|ChatId: {callback.Message.Chat.Id}");
        sb.Append($"|MessageType: {callback.Message.Type}");
        sb.Append($"|DateTime: {callback.Message.Date}");
        if (callback.Message.From != null) sb.Append($"|UserFrom: {callback.Message.From}");
        
        return sb.ToString();
    }
}