using System.Text.Json.Serialization;

namespace TestWebTgBot.Models.TgModels;

public class PhotoSize
{
    [JsonPropertyName("file_id")]
    public string? FileId { get; set; }
}