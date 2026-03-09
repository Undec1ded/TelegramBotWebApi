namespace TestWebTgBot.Models.Dbo;

public class QuizQuestionEntity
{
    public long Id { get; set; }

    public int QuizId { get; set; }

    public string Question { get; set; } = null!;

    public string? ImagesIds { get; set; }

    public List<VariantEntity>? Variants { get; set; }

}
