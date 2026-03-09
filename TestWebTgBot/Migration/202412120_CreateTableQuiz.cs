using FluentMigrator;

namespace TestWebTgBot.Migration;

[Migration(202412120)]
public class CreateTableQuiz : FluentMigrator.Migration
{
    public override void Up()
    {
        Create.Table("Quiz")
            .WithColumn("Id").AsInt32().PrimaryKey().Identity()
            .WithColumn("Active").AsBoolean().NotNullable().WithDefaultValue(false)
            .WithColumn("Title").AsString().NotNullable()
            .WithColumn("StartTime").AsDateTime().Nullable()
            .WithColumn("EndTime").AsDateTime().Nullable();
    }

    public override void Down()
    {
        Delete.Table("Quiz");
    }
}
