using FluentMigrator;

namespace TestWebTgBot.Migration;

[Migration(202412132)]
public class AddedColumnEditingQuizQuestionId : FluentMigrator.Migration
{
    public override void Up()
    {
        Alter.Table("Users")
            .AddColumn("EditingQuizQuestionId").AsInt64().Nullable();
    }

    public override void Down()
    {
        Delete.Column("EditingQuizQuestionId").FromTable("Users");
    }
}
