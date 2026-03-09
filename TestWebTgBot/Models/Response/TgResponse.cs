using System.Text.Json.Serialization;

namespace TestWebTgBot.Models.Response;

public class TgResponse<TResponse>
{
    [JsonPropertyName("ok")]
    public bool Ok { get; set; }

    [JsonPropertyName("result")]
    public TResponse? Result { get; set; }    

    [JsonPropertyName("error_code")]
    public int? ErrorCode { get; set; }
        
    [JsonPropertyName("description")]
    public string? ErrorDescription { get; set; }
}