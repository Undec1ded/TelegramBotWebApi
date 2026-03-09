using System.Text.Json.Serialization;

namespace TestWebTgBot.Models.Response;

public class DeleteMessageResponse
{
    [JsonPropertyName("ok")]
    public bool Ok { get; set; }

    [JsonPropertyName("result")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Result { get; set; }
}