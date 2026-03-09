using System.Text.Json.Serialization;

namespace TestWebTgBot.Models.Requests;

public class DeleteMessagesRequest
{
    [JsonPropertyName("chat_id")]
    public long ChatId { get; set; }
    
    [JsonPropertyName("message_ids")]
    public List<int> MessageIds { get; set; } = null!;
}