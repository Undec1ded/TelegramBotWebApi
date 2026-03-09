namespace TestWebTgBot.Models.Dbo;

public class ResultQuizEntity
{
    public int Id { get; set; }
    
    public string UserFullName { get; set; } = null!;
    
    public int TotalPoints { get; set; } 
}