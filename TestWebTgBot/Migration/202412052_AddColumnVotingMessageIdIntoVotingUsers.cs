using FluentMigrator;

namespace TestWebTgBot.Migration;

[Migration(202412052)]
public class AddColumnVotingMessageIdIntoVotingUsers : FluentMigrator.Migration
{
    public override void Up()
    {
        Alter.Table("VotingUsers").AddColumn("VotingMessageId").AsInt32().Nullable();
    }

    public override void Down()
    {
        Delete.Column("VotingMessageId").FromTable("VotingUsers");
    }
}