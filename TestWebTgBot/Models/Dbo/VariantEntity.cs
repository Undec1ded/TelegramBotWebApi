namespace TestWebTgBot.Models.Dbo;

public class VariantEntity
{
    public long Id { get; set; }

    public string Text { get; set; } = null!;

    public bool IsCorrect { get; set; }

    public long QuestionId { get; set; }
}
