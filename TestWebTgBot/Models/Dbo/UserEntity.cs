using TestWebTgBot.Models.Internal;

namespace TestWebTgBot.Models.Dbo;

public class UserEntity
{
    public long Id { get; set; }
    
    public bool IsAdmin { get; set; } = false;

    public bool IsSubscribed { get; set; } = false;
    
    public string? UserFullName { get; set; }
    
    public bool IsQuizStart { get; set; }
    
    public UserChatStates? UserChatState { get; set; }
    
}