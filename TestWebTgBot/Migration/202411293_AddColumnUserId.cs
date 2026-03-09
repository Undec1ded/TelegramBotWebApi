using FluentMigrator;

namespace TestWebTgBot.Migration;

[Migration(202411293)]
public class AddColumnUserId : FluentMigrator.Migration
{
    public override void Up()
    {
        Alter.Table("Questions").AddColumn("UserId").AsInt64();
        Create.ForeignKey().FromTable("Questions").ForeignColumn("UserId").ToTable("Users").PrimaryColumn("Id");
    }

    public override void Down()
    {
        Delete.Column("UserId").FromTable("Questions");
    }
}