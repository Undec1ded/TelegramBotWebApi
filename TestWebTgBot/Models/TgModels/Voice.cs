using System.Text.Json.Serialization;

namespace TestWebTgBot.Models.TgModels;

public class Voice
{
    [JsonPropertyName("file_id")]
    public string FileId {get; set;} = null!;
    
    [JsonPropertyName("file_unique_id")]
    public string FileUniqueId {get; set;} = null!;
    
    [JsonPropertyName("duration")]
    public int Duration {get; set;}
}