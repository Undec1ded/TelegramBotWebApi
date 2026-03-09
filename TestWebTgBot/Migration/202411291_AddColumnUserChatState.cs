using FluentMigrator;

namespace TestWebTgBot.Migration;

[Migration(202411291)]
public class AddColumnUserChatState : FluentMigrator.Migration
{
    public override void Up()
    {
        Alter.Table("Users").AddColumn("UserChatState").AsInt16().NotNullable().WithDefaultValue(0);
    }

    public override void Down()
    {
        Delete.Column("UserChatState").FromTable("Users");
    }
}