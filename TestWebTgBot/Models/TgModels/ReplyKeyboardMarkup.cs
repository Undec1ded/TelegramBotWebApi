using System.Text.Json.Serialization;

namespace TestWebTgBot.Models.TgModels;

public class ReplyKeyboardMarkup
{
    [JsonPropertyName("keyboard")]
    public List<List<KeyboardButton>> ReplyKeyboard { get; set; } = new List<List<KeyboardButton>>();
}