using System.Text.Json.Serialization;

namespace TestWebTgBot.Models.TgModels;

public class User
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("is_bot")]
    public bool? IsBot { get; set; }

    [JsonPropertyName("first_name")]
    public string? FirstName { get; set; }

    [JsonPropertyName("username")]
    public string? UserName { get; set; }

    [JsonPropertyName("can_join_groups")]
    public bool? CanJoinGroups { get; set; }

    [JsonPropertyName("can_read_all_group_messages")]
    public bool? CanReadAllGroupMessages { get; set; }

    [JsonPropertyName("supports_inline_queries")]
    public bool? SupportsInlineQueries { get; set; }

    [JsonPropertyName("can_connect_to_business")]
    public bool? CanConnectToBusiness { get; set; }

    [JsonPropertyName("has_main_web_app")]
    public bool? HasMainWebApp { get; set; }
}
