using System.Text.Json.Serialization;

namespace TestWebTgBot.Models.Requests;

public class GetUpdatesRequest
{
    [JsonPropertyName("offset")]
    public int Offset { get; set; }

    [JsonPropertyName("limit")]
    public int Limit { get; set; }

    [JsonPropertyName("timeout")]
    public int Timeout { get; set;}
}