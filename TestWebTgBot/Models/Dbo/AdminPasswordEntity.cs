namespace TestWebTgBot.Models.Dbo;

public class AdminPasswordEntity
{
    public int Id { get; set; }

    public string Password { get; set; } = null!;
    
    public DateTime TimeCreated { get; set; }
}