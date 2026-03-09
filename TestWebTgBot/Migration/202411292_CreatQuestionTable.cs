using FluentMigrator;

namespace TestWebTgBot.Migration;

[Migration(202411292)]
public class CreatQuestionTable : FluentMigrator.Migration
{
    public override void Up()
    {
        Create.Table("Questions")
            .WithColumn("Id").AsInt64().PrimaryKey().Identity()
            .WithColumn("Question").AsString();
    }

    public override void Down()
    {
        Delete.Table("Questions");
    }
}