namespace TestWebTgBot.Models.Dbo;

public class QuizAdminEntity
{
    public int Id { get; set; }

    public bool IsStart { get; set; }

    public DateTime? StartTime { get; set; }

    public DateTime? EndTime { get; set; }
}