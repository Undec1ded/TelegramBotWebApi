using System.Text.Json.Serialization;
using TestWebTgBot.Models.TgModels;

namespace TestWebTgBot.Models.Requests;

public class EditMessageTextRequest
{
    [JsonPropertyName("chat_id")]
    public long ChatId { get; set; }
    
    [JsonPropertyName("message_id")]
    public int MessageId { get; set; }
        
    [JsonPropertyName("text")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Text { get; set; }
    
    [JsonPropertyName("reply_markup")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public InlineKeyboardMarkup? ReplyMarkup { get; set; }
}