using System.Text.Json.Serialization;

namespace TestWebTgBot.Models.TgModels;

public class InlineKeyboardMarkup
{
    [JsonPropertyName("inline_keyboard")]
    public List<List<InlineKeyboardButton>> InlineKeyboard { get; set; } = new List<List<InlineKeyboardButton>>();
}