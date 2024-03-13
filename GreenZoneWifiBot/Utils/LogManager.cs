using System.Text;
using Telegram.Bot.Types;

namespace GreenZoneWifiBot.Utils;

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
}