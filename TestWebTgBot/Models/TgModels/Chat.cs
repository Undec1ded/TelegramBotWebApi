using System.Text.Json.Serialization;

namespace TestWebTgBot.Models.TgModels;

public class Chat
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("type")]
    public string? Type { get; set; } //mb bage

    [JsonPropertyName("title")]
    public string? Title { get; set;}
}