namespace TestWebTgBot.Models.Dbo;

public class QuestionEntity
{
    public long Id { get; set; }
    
    public string? Question { get; set; }
    
    public long UserId { get; set; }
}