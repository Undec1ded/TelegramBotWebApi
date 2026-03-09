using System.Text.Json.Serialization;

namespace TestWebTgBot.Models.TgModels;

public class Message
{
    [JsonPropertyName("message_id")]
    public int MessageId {get; set;}

    [JsonPropertyName("text")]
    public string? Text {get; set;}

    [JsonPropertyName("caption")]
    public string? Caption {get; set;}

    [JsonPropertyName("message_thread_id")]
    public int MessageThreadId {get; set;}

    [JsonPropertyName("from")]
    public 	User? From {get; set;}

    [JsonPropertyName("sender_chat")]
    public Chat? SenderChat { get; set; }

    [JsonPropertyName("date")]
    public long Date { get; set; }
    
    [JsonPropertyName("photo")]
    public List<PhotoSize>? Photo { get; set; }
    
    [JsonPropertyName("sticker")]
    public Sticker?  Sticker { get; set; }
    
    [JsonPropertyName("voice")]
    public Voice? Voice { get; set; }
}
