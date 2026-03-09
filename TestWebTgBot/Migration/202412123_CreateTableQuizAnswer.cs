using System.Data;
using FluentMigrator;

namespace TestWebTgBot.Migration;

[Migration(202412123)]
public class CreateTableQuizAnswer : FluentMigrator.Migration
{
    public override void Up()
    {
        Create.Table("UserQuizAnswer")
            .WithColumn("Id").AsInt64().PrimaryKey().Identity()
            .WithColumn("UserId").AsInt64().NotNullable()
            .WithColumn("QuestionId").AsInt64().NotNullable()
            .WithColumn("VariantId").AsInt64().NotNullable()
            .WithColumn("MessageId").AsInt32().Nullable();

        Create.ForeignKey("FK_UserQuizAnswer_UserId")
            .FromTable("UserQuizAnswer").ForeignColumn("UserId")
            .ToTable("Users").PrimaryColumn("Id")
            .OnDelete(Rule.Cascade);

        Create.ForeignKey("FK_UserQuizAnswer_QuestionId")
            .FromTable("UserQuizAnswer").ForeignColumn("QuestionId")
            .ToTable("QuizQuestion").PrimaryColumn("Id")
            .OnDelete(Rule.Cascade);

        Create.ForeignKey("FK_UserQuizAnswer_VariantId")
            .FromTable("UserQuizAnswer").ForeignColumn("VariantId")
            .ToTable("QuizVariant").PrimaryColumn("Id");
    }

    public override void Down()
    {
        Delete.Table("UserQuizAnswer");
    }
}
