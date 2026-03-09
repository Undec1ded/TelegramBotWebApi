using FluentMigrator;

namespace TestWebTgBot.Migration;

[Migration(202412130)]
public class DropTitleColumnFromQuizTable : FluentMigrator.Migration
{
    public override void Up()
    {
        Delete.Column("Title").FromTable("Quiz");
    }

    public override void Down()
    {
        Alter.Table("Quiz")
            .AddColumn("Title").AsString().NotNullable();
    }
}
