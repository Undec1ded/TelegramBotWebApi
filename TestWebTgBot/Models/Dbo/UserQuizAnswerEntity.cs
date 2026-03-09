namespace TestWebTgBot.Models.Dbo;

public class UserQuizAnswerEntity
{
    public long Id { get; set; }

    public long UserId { get; set; }

    public long QuestionId { get; set; }

    public long VariantId { get; set; }

    public int? MessageId { get; set; }
}
