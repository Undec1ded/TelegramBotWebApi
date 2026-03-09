using FluentMigrator;

namespace TestWebTgBot.Migration;

[Migration(202412040)]
public class CreateTableVoting : FluentMigrator.Migration
{
    public override void Up()
    {
        Create.Table("Voting")
            .WithColumn("Id").AsInt32().PrimaryKey().Identity()
            .WithColumn("Question").AsString().NotNullable()
            .WithColumn("OptionFirst").AsString().NotNullable()
            .WithColumn("OptionSecond").AsString().NotNullable()
            .WithColumn("Result").AsDouble().Nullable()
            .WithColumn("IsStart").AsBoolean().NotNullable().WithDefaultValue(false);
    }

    public override void Down()
    {
        Delete.Table("Voting");
    }
}