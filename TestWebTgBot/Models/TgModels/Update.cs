using System.Text.Json.Serialization;
using TestWebTgBot.Models.Dbo;
using TestWebTgBot.Models.Internal;

namespace TestWebTgBot.Models.TgModels;

public class Update
{
    [JsonPropertyName("update_id")]
    public int UpdateId { get; set; }

    [JsonPropertyName("message")]
    public Message? Message { get; set; }

    [JsonPropertyName("edited_message")]
    public Message? EditedMessage { get; set; }
    
    [JsonPropertyName("callback_query")]
    public CallbackQuery? CallbackQuery { get; set; }
    
    [JsonIgnore]
    public UserData UserData { get; set; }
    
}