using FluentMigrator;

namespace TestWebTgBot.Migration;

[Migration(202412100)]
public class CreateTableQuizAdmin : FluentMigrator.Migration
{
    public override void Up()
    {
        Create.Table("QuizAdmin")
            .WithColumn("Id").AsInt32().PrimaryKey().Identity()
            .WithColumn("IsStart").AsBoolean().NotNullable().WithDefaultValue(false)
            .WithColumn("StartTime").AsDateTime().Nullable()
            .WithColumn("EndTime").AsDateTime().Nullable();
    }

    public override void Down()
    {
        Delete.Table("QuizAdmin");
    }
}