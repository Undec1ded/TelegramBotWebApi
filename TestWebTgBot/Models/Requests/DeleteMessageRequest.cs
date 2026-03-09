using System.Text.Json.Serialization;

namespace TestWebTgBot.Models.Requests;

public class DeleteMessageRequest
{
    [JsonPropertyName("chat_id")]
    public long ChatId { get; set;}

    [JsonPropertyName("message_id")]
    public int MessageId { get; set;}
}