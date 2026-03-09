using System.Data;
using FluentMigrator;

namespace TestWebTgBot.Migration;

[Migration(202412121)]
public class CreateTableQuizQuestion : FluentMigrator.Migration
{
    public override void Up()
    {
        Create.Table("QuizQuestion")
            .WithColumn("Id").AsInt64().PrimaryKey().Identity()
            .WithColumn("QuizId").AsInt32().NotNullable()
            .WithColumn("Question").AsString().NotNullable();

        Create.ForeignKey("FK_QuizQuestion_QuizId")
            .FromTable("QuizQuestion").ForeignColumn("QuizId")
            .ToTable("Quiz").PrimaryColumn("Id")
            .OnDelete(Rule.Cascade);
    }

    public override void Down()
    {
        Delete.Table("QuizQuestion");
    }
}
