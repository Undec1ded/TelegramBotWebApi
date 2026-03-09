using System.Text.Json.Serialization;

namespace TestWebTgBot.Models.TgModels;

public class KeyboardButton
{
    [JsonPropertyName("text")]
    public string Text { get; set; } = null!;
}
