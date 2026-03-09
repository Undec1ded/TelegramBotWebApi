using FluentMigrator;

namespace TestWebTgBot.Migration;

[Migration(202412070)]
public class AddColumnIsNotifiedIntoEventEntries : FluentMigrator.Migration
{
    public override void Up()
    {
        Alter.Table("EventEntries").AddColumn("IsNotified").AsBoolean().NotNullable().WithDefaultValue(false);
    }

    public override void Down()
    {
        Delete.Column("VotingMessageId").FromTable("EventEntries");
    }
}