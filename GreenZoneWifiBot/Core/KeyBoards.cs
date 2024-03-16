using Telegram.Bot.Types.ReplyMarkups;

namespace GreenZoneWifiBot.Core;

public static class KeyBoards
{
    public static readonly KeyboardButton[] IndexKeyBoard =
    {
        new(KeyBoardButtons.accountButtonText),
        new(KeyBoardButtons.helpButtonText)
    };
    
    public static readonly InlineKeyboardButton[] ShowKeyBoard =
    {
        InlineKeyboardButton.WithCallbackData(text: KeyBoardButtons.downloadButtonText, callbackData: "/download"),
    };
    
    public static readonly InlineKeyboardButton[] DownloadKeyBoard =
    {
        InlineKeyboardButton.WithCallbackData(text: KeyBoardButtons.downloadJsonButtonText, callbackData: "/downloadJson"),
        InlineKeyboardButton.WithCallbackData(text: KeyBoardButtons.downloadCsvButtonText, callbackData: "/downloadCsv"),
    };
    
    public static readonly InlineKeyboardButton[][] FileWorkKeyBoard =
    {
        new[] 
        { 
            InlineKeyboardButton.WithCallbackData(text: KeyBoardButtons.sortButtonText, callbackData: "/sort"),
            InlineKeyboardButton.WithCallbackData(text: KeyBoardButtons.chooseButtonText, callbackData: "/choose")
        },
        new[]
        {
            InlineKeyboardButton.WithCallbackData(text: KeyBoardButtons.showButtonText, callbackData: "/show"),
            InlineKeyboardButton.WithCallbackData(text: KeyBoardButtons.downloadButtonText, callbackData: "/download")
        }
    };
    
    public static readonly InlineKeyboardButton[][] AfterShowingDataKeyBoard =
    {
        new[]
        {
            InlineKeyboardButton.WithCallbackData(text: KeyBoardButtons.downloadButtonText, callbackData: "/download")
        },
        new[] 
        { 
            InlineKeyboardButton.WithCallbackData(text: KeyBoardButtons.sortButtonText, callbackData: "/sort"),
            InlineKeyboardButton.WithCallbackData(text: KeyBoardButtons.chooseButtonText, callbackData: "/choose")
        }
    };
    
    public static readonly InlineKeyboardButton[][] Fields =
    {
        new[] { InlineKeyboardButton.WithCallbackData(text: "ID", callbackData: "ID")},
        new[] { InlineKeyboardButton.WithCallbackData(text: "global_id", callbackData: "global_id")},
        new[] { InlineKeyboardButton.WithCallbackData(text: "Name", callbackData: "Name")},
        new[] { InlineKeyboardButton.WithCallbackData(text: "AdmArea", callbackData: "AdmArea")},
        new[] { InlineKeyboardButton.WithCallbackData(text: "District", callbackData: "District")},
        new[] { InlineKeyboardButton.WithCallbackData(text: "ParkName", callbackData: "ParkName")},
        new[] { InlineKeyboardButton.WithCallbackData(text: "WifiName", callbackData: "WifiName")},
        new[] { InlineKeyboardButton.WithCallbackData(text: "CoverageArea", callbackData: "CoverageArea")},
        new[] { InlineKeyboardButton.WithCallbackData(text: "FunctionFlag", callbackData: "FunctionFlag")},
        new[] { InlineKeyboardButton.WithCallbackData(text: "AccessFlag", callbackData: "AccessFlag")},
        new[] { InlineKeyboardButton.WithCallbackData(text: "Password", callbackData: "Password")},
        new[] { InlineKeyboardButton.WithCallbackData(text: "Longitude_WGS84", callbackData: "Longitude_WGS84")},
        new[] { InlineKeyboardButton.WithCallbackData(text: "Latitude_WGS84", callbackData: "Latitude_WGS84")},
        new[] { InlineKeyboardButton.WithCallbackData(text: "geodata_center", callbackData: "geodata_center")},
        new[] { InlineKeyboardButton.WithCallbackData(text: "geoarea", callbackData: "geoarea")}
    };
    
    public static readonly InlineKeyboardButton[][] FavouriteParksKeyBoard =
    {
        new[] { InlineKeyboardButton.WithCallbackData(text: "1", callbackData: "11")},
        new[] { InlineKeyboardButton.WithCallbackData(text: "2", callbackData: "11")},
        new[] { InlineKeyboardButton.WithCallbackData(text: "3", callbackData: "11")},
        new[] { InlineKeyboardButton.WithCallbackData(text: "4", callbackData: "11")},
        new[] { InlineKeyboardButton.WithCallbackData(text: "5", callbackData: "11")},
        new[] { InlineKeyboardButton.WithCallbackData(text: "6", callbackData: "11")},
        new[] { InlineKeyboardButton.WithCallbackData(text: "7", callbackData: "11")},
        new[] { InlineKeyboardButton.WithCallbackData(text: "8", callbackData: "11")}
    };
}