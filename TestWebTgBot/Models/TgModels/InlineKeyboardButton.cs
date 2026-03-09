using System.Text.Json.Serialization;

namespace TestWebTgBot.Models.TgModels;

public class InlineKeyboardButton
{
    [JsonPropertyName("text")]
    public string Text {get; set;} = null!;

    [JsonPropertyName("callback_data")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? CallbackData { get; set; }
    
    [JsonPropertyName("url")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Url { get; set; }
}