using System.Data;
using FluentMigrator;

namespace TestWebTgBot.Migration;

[Migration(202412122)]
public class CreateTableQuizVariant  : FluentMigrator.Migration
{
    public override void Up()
    {
        Create.Table("QuizVariant")
            .WithColumn("Id").AsInt64().PrimaryKey().Identity()
            .WithColumn("Text").AsString().NotNullable()
            .WithColumn("IsCorrect").AsBoolean().NotNullable().WithDefaultValue(false)
            .WithColumn("QuestionId").AsInt64().NotNullable();

        Create.ForeignKey("FK_QuizVariant_QuestionId")
            .FromTable("QuizVariant").ForeignColumn("QuestionId")
            .ToTable("QuizQuestion").PrimaryColumn("Id")
            .OnDelete(Rule.Cascade);
    }

    public override void Down()
    {
        Delete.Table("QuizVariant");
    }
}
