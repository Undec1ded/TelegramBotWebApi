namespace TestWebTgBot.Models.Dbo;

public class VotingEntity
{
    public int Id { get; set; }
    
    public string? Question { get; set; }
    
    public string? OptionFirst { get; set; }
    
    public string? OptionSecond { get; set; }
    
    public double Result { get; set; }
    
    public bool IsStart { get; set; } 
}