using System.Text.Json.Serialization;

namespace TestWebTgBot.Models.Requests;

public class SendPhotoRequest
{
    [JsonPropertyName("chat_id")]
    public long ChatId { get; set; }

    [JsonPropertyName("photo")]
    public string Photo { get; set; } = null!;
    
    [JsonPropertyName("caption")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Caption { get; set; }
}