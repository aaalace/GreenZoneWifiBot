using GreenZoneWifiBot.Core;
using GreenZoneWifiBot.Utils;
using Lib;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace GreenZoneWifiBot.Services.CommandActions;

public static partial class Actions
{
    public static async Task SortAction(ITelegramBotClient botClient, CallbackQuery callback, CancellationToken cts)
    {
        if (callback.Message == null)
        {
            await ErrorAction(botClient, callback, cts);
            return;
        }
        
        await botClient.SendTextMessageAsync(
            chatId: callback.Message.Chat.Id,
            text: "Choose field to sort by",
            replyMarkup: new InlineKeyboardMarkup(KeyBoards.Fields),
            cancellationToken: cts);
    }

    public static async Task SortFieldAction(string field, ITelegramBotClient botClient, CallbackQuery callback, 
        CancellationToken cts)
    {
        if (callback.Message == null)
        {
            await ErrorAction(botClient, callback, cts);
            return;
        }
        
        var dirPath = PathGetter.Get("uploads");
        var chatId = callback.Message.Chat.Id;
        var fullPath = Path.Combine(dirPath, chatId.ToString());

        if (!System.IO.File.Exists(fullPath))
        {
            await ErrorAction(botClient, callback, cts, "File was not uploaded yet, maybe it had a wrong format");
            return;
        }

        var stream = System.IO.File.OpenRead(fullPath);
        var json = new JsonProcessing();
        var collection = await json.Read(stream);
        stream.Close();
        if (!json.State)
        {
            await ErrorAction(botClient, callback, cts, "Something went wrong while sorting, try again later");
        }

        var sortedCollection = field switch
        {
            "ID" => collection.OrderBy(x => x.Id).ToList(),
            "global_id" => collection.OrderBy(x => x.GlobalId).ToList(),
            "Name" => collection.OrderBy(x => x.Name).ToList(),
            "AdmArea" => collection.OrderBy(x => x.AdmArea).ToList(),
            "District" => collection.OrderBy(x => x.District).ToList(),
            "ParkName" => collection.OrderBy(x => x.ParkName).ToList(),
            "WifiName" => collection.OrderBy(x => x.WifiName).ToList(),
            "CoverageArea" => collection.OrderBy(x => x.CoverageArea).ToList(),
            "FunctionFlag" => collection.OrderBy(x => x.FunctionFlag).ToList(),
            "AccessFlag" => collection.OrderBy(x => x.AccessFlag).ToList(),
            "Password" => collection.OrderBy(x => x.Password).ToList(),
            "Longitude_WGS84" => collection.OrderBy(x => x.Longitude).ToList(),
            "Latitude_WGS84" => collection.OrderBy(x => x.Latitude).ToList(),
            "geodata_center" => collection.OrderBy(x => x.GeodataCentre).ToList(),
            "geoarea" => collection.OrderBy(x => x.GeoArea).ToList(),
            _ => collection
        };

        var jsonAfterSort = new JsonProcessing();
        var streamAfterSort = await jsonAfterSort.Write(sortedCollection, fullPath);
        streamAfterSort.Close();
        
        await botClient.SendTextMessageAsync(
            chatId: callback.Message.Chat.Id,
            text: $"Data successfully sorted by <b>{field}</b>, what's next?",
            parseMode: ParseMode.Html,
            replyMarkup: new InlineKeyboardMarkup(KeyBoards.FileWorkKeyBoard),
            cancellationToken: cts);
    }
}