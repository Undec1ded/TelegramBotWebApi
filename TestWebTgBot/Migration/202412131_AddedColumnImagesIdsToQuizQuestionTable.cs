using FluentMigrator;

namespace TestWebTgBot.Migration;

[Migration(202412131)]
public class AddedColumnImagesIdsToQuizQuestionTable: FluentMigrator.Migration
{
    public override void Up()
    {
        Alter.Table("QuizQuestion")
            .AddColumn("ImagesIds").AsString().Nullable();
    }

    public override void Down()
    {
        Delete.Column("ImagesIds").FromTable("QuizQuestion");
    }
}
