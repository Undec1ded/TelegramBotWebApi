using System.Text.Json.Serialization;

namespace TestWebTgBot.Models.TgModels;

public class CallbackQuery
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = null!;

    [JsonPropertyName("from")]
    public User From { get; set; } = null!;
    
    [JsonPropertyName("message")]
    public Message? Message { get; set; }

    [JsonPropertyName("data")]
    public string? Data { get; set;}

}
