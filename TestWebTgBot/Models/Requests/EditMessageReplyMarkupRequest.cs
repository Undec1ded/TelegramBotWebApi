using System.Text.Json.Serialization;
using TestWebTgBot.Models.TgModels;

namespace TestWebTgBot.Models.Requests;

public class EditMessageReplyMarkupRequest
{
    [JsonPropertyName("chat_id")]
    public long ChatId { get; set; }
    
    [JsonPropertyName("message_id")]
    public int MessageId { get; set; }
    
    [JsonPropertyName("reply_markup")]
    public InlineKeyboardMarkup? ReplyMarkup { get; set; }
    
}