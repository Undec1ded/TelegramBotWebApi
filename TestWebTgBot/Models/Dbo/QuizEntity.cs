namespace TestWebTgBot.Models.Dbo;

public class QuizEntity
{
    public int Id { get; set; }

    public bool Active { get; set; } = false;

    public DateTime? StartTime { get; set; }

    public DateTime? EndTime { get; set; }

    public List<QuizQuestionEntity>? Questions { get; set; }
}
