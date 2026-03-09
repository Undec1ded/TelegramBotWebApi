using FluentMigrator;

namespace TestWebTgBot.Migration;

[Migration(202412050)]
public class CreateTableVotingUsers : FluentMigrator.Migration
{
    public override void Up()
    {
        Create.Table("VotingUsers")
            .WithColumn("Id").AsInt32().PrimaryKey().Identity()
            .WithColumn("VotingId").AsInt32().NotNullable()
            .WithColumn("UserId").AsInt64().NotNullable()
            .WithColumn("Result").AsBoolean().NotNullable();
    }
    public override void Down()
    {
        Delete.Table("VotingUsers");
    }
}