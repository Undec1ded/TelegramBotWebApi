using TestWebTgBot.Models.Dbo;

namespace TestWebTgBot.Models.Internal;

public class UserData
{
    public long Id { get; set; }
    public bool IsSubscribed { get; set; }
    public bool IsAdmin { get; set; }
    
    public string? UserFullName { get; set; }

    public bool IsQuizStart { get; set; }
    public UserChatStates? UserChatState { get; set; }
}