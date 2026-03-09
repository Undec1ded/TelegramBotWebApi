namespace TestWebTgBot.Models.Dbo;

public class UserQuestionEntity
{
    public long UserId { get; set; }
    
    public string? UserFullName { get; set; }
    
    public string? Question { get; set; }
    
}