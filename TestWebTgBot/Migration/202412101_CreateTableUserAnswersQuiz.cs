using FluentMigrator;

namespace TestWebTgBot.Migration;

[Migration(202412101)]
public class CreateTableUserAnswersQuiz : FluentMigrator.Migration
{
    public override void Up()
    {
        Create.Table("UserAnswersQuiz")
            .WithColumn("Id").AsInt32().PrimaryKey().Identity()
            .WithColumn("QuizId").AsInt32().NotNullable()
            .WithColumn("UserId").AsInt64().NotNullable()
            .WithColumn("TotalPoints").AsInt32().NotNullable().WithDefaultValue(0)
            .WithColumn("EndQuizMessageId").AsInt32().Nullable()
            .WithColumn("Answer1").AsInt32().Nullable()
            .WithColumn("Question1MessageId").AsInt32().Nullable()
            .WithColumn("Answer2").AsInt32().Nullable()
            .WithColumn("Question2MessageId").AsInt32().Nullable()
            .WithColumn("Answer3").AsInt32().Nullable()
            .WithColumn("Question3MessageId").AsInt32().Nullable()
            .WithColumn("Answer4").AsInt32().Nullable()
            .WithColumn("Question4MessageId").AsInt32().Nullable()
            .WithColumn("Answer5").AsInt32().Nullable()
            .WithColumn("Question5MessageId").AsInt32().Nullable()
            .WithColumn("Answer6").AsInt32().Nullable()
            .WithColumn("Question6MessageId").AsInt32().Nullable()
            .WithColumn("Answer7").AsInt32().Nullable()
            .WithColumn("Question7MessageId").AsInt32().Nullable()
            .WithColumn("Answer8").AsInt32().Nullable()
            .WithColumn("Question8MessageId").AsInt32().Nullable()
            .WithColumn("Answer9").AsInt32().Nullable()
            .WithColumn("Question9MessageId").AsInt32().Nullable()
            .WithColumn("Answer10").AsInt32().Nullable()
            .WithColumn("Question10MessageId").AsInt32().Nullable()
            .WithColumn("Answer11").AsInt32().Nullable()
            .WithColumn("Question11MessageId").AsInt32().Nullable()
            .WithColumn("Answer12").AsInt32().Nullable()
            .WithColumn("Question12MessageId").AsInt32().Nullable();
        
        Create.Index("IX_UserAnswersQuiz_QuizId").OnTable("UserAnswersQuiz").OnColumn("QuizId");
        Create.Index("IX_UserAnswersQuiz_UserId").OnTable("UserAnswersQuiz").OnColumn("UserId");

        Create.ForeignKey("FK_UserAnswersQuiz_QuizAdmin")
            .FromTable("UserAnswersQuiz").ForeignColumn("QuizId")
            .ToTable("QuizAdmin").PrimaryColumn("Id");

        Create.ForeignKey("FK_UserAnswersQuiz_Users")
            .FromTable("UserAnswersQuiz").ForeignColumn("UserId")
            .ToTable("Users").PrimaryColumn("Id");
    }

    public override void Down()
    {
        Delete.Table("UserAnswersQuiz");
    }
}