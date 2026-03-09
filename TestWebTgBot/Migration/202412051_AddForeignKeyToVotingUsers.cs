using FluentMigrator;

namespace TestWebTgBot.Migration;

[Migration(202412051)]
public class AddForeignKeyToVotingUsers : FluentMigrator.Migration
{
    public override void Up()
    {
        Create.ForeignKey()
            .FromTable("VotingUsers").ForeignColumn("VotingId")
            .ToTable("Voting").PrimaryColumn("Id");
    }

    public override void Down()
    {
        Delete.ForeignKey()
            .FromTable("VotingUsers").ForeignColumn("VotingId");
    }
}