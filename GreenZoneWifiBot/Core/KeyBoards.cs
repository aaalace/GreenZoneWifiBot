using Telegram.Bot.Types.ReplyMarkups;

namespace GreenZoneWifiBot.Core;

public static class KeyBoards
{
    public static readonly KeyboardButton[] IndexKeyBoard =
    {
        new(KeyBoardButtons.accountButtonText),
        new(KeyBoardButtons.helpButtonText)
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