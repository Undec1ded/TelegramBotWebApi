using System.Text.Json.Serialization;

namespace TestWebTgBot.Models.TgModels;

public class Sticker
{
    [JsonPropertyName("file_id")]
    public string FileId {get; set;} = null!;
    
    [JsonPropertyName("file_unique_id")]
    public string FileUniqueId {get; set;} = null!;
    
    [JsonPropertyName("type")]
    public string Type {get; set;} = null!;
    
    [JsonPropertyName("width")]
    public int Width {get; set;}
    
    [JsonPropertyName("height")]
    public int Height {get; set;}
    
    [JsonPropertyName("is_animated")]
    public bool IsAnimated {get; set;}
    
    [JsonPropertyName("is_video")]
    public bool IsVideo {get; set;}
}