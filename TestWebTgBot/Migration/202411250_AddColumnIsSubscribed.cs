using FluentMigrator;

namespace TestWebTgBot.Migration;

[Migration(202411250)]
public class AddColumnIsSubscribed : FluentMigrator.Migration
{
    public override void Up()
    {
        Alter.Table("Users").AddColumn("IsSubscribed").AsBoolean().NotNullable().WithDefaultValue(false);
    }

    public override void Down()
    {
        Delete.Column("IsSubscribed").FromTable("Users");
    }
}