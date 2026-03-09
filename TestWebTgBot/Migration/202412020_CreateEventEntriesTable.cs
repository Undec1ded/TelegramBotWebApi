using FluentMigrator;

namespace TestWebTgBot.Migration;

[Migration(202412020)]
public class CreateEventEntriesTable : FluentMigrator.Migration 
{
    public override void Up()
    {
        Create.Table("EventEntries")
            .WithColumn("Id").AsInt32().PrimaryKey().Identity()
            .WithColumn("IdEvent").AsInt32().NotNullable()
            .WithColumn("UserId").AsInt64().NotNullable();
    }

    public override void Down()
    {
        Delete.Table("EventEntries");
    }
}