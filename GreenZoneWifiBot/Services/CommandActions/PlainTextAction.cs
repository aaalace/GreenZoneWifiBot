using GreenZoneWifiBot.Core;
using GreenZoneWifiBot.Utils;
using Lib;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using File = System.IO.File;

namespace GreenZoneWifiBot.Services.CommandActions;

public static partial class Actions
{
    /// <summary>
    /// Action that gives "idk whats that" error if it's so. Handling choice value.
    /// </summary>
    public static async Task PlainTextAction(
        ITelegramBotClient botClient, Message message, CancellationToken cts)
    {
        var dirPath = PathGetter.Get("tmpChoice");
        var chatId = message.Chat.Id;
        var fullChoicePath = Path.Combine(dirPath, chatId.ToString());
        
        // Important! Every action error goes here.
        if (!File.Exists(fullChoicePath))
        {
            await ErrorMessageAction(botClient, message, cts);
            return;
        }
        
        string? value = message.Text;
        if (value == null)
        {
            await ErrorMessageAction(botClient, message, cts, "I can choose only by text value, I am not AI yet :(");
            return;
        }
        
        var dirUploadsPath = PathGetter.Get("uploads");
        var fullUploadsPath = Path.Combine(dirUploadsPath, chatId.ToString());

        if (!File.Exists(fullUploadsPath))
        {
            await ErrorMessageAction(botClient, message, cts, "File was not uploaded yet, maybe it had a wrong format");
            return;
        }
        
        var stream = File.OpenRead(fullUploadsPath);
        var json = new JsonProcessing();
        var collection = await json.Read(stream);
        stream.Close();
        if (!json.State)
        {
            await ErrorMessageAction(botClient, message, cts, "Something went wrong while choosing, try again later");
        }

        string fieldChoice = await File.ReadAllTextAsync(fullChoicePath, cts);
        var filteredCollection = fieldChoice switch
        {
            "ID_choice" => collection.Where(x => x.Id == value).ToList(),
            "global_id_choice" => collection.Where(x => x.GlobalId == value).ToList(),
            "Name_choice" => collection.Where(x => x.Name == value).ToList(),
            "AdmArea_choice" => collection.Where(x => x.AdmArea == value).ToList(),
            "District_choice" => collection.Where(x => x.District == value).ToList(),
            "ParkName_choice" => collection.Where(x => x.ParkName == value).ToList(),
            "WifiName_choice" => collection.Where(x => x.WifiName == value).ToList(),
            "CoverageArea_choice" => collection.Where(x => x.CoverageArea == value).ToList(),
            "FunctionFlag_choice" => collection.Where(x => x.FunctionFlag == value).ToList(),
            "AccessFlag_choice" => collection.Where(x => x.AccessFlag == value).ToList(),
            "Password_choice" => collection.Where(x => x.Password == value).ToList(),
            "Longitude_WGS84_choice" => collection.Where(x => x.Longitude == value).ToList(),
            "Latitude_WGS84_choice" => collection.Where(x => x.Latitude == value).ToList(),
            "geodata_center_choice" => collection.Where(x => x.GeodataCentre == value).ToList(),
            "geoarea_choice" => collection.Where(x => x.GeoArea == value).ToList(),
            _ => collection
        };

        var jsonAfterSort = new JsonProcessing(fullUploadsPath);
        var streamAfterSort = await jsonAfterSort.Write(filteredCollection);
        streamAfterSort.Close();

        File.Delete(fullChoicePath);
        
        await botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: $"Data was successfully choosed by <b>{fieldChoice.Replace("_choice", "")}</b> and value <b>{value}</b>, what's next?",
            parseMode: ParseMode.Html,
            replyMarkup: new InlineKeyboardMarkup(KeyBoards.FileWorkKeyBoard),
            cancellationToken: cts);
    }
}