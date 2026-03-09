namespace TestWebTgBot.Models.Dbo;

public class UserEventsEntity
{
    public int Id { get; set; }

    public long UserId { get; set; }

    public int IdEvent { get; set; }

    public bool IsNotified { get; set; }
}