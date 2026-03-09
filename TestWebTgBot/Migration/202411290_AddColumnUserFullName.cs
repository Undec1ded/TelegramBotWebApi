using FluentMigrator;

namespace TestWebTgBot.Migration;
[Migration(202411290)]
public class AddColumnUserFullName : FluentMigrator.Migration
{
    public override void Up()
    {
        Alter.Table("Users").AddColumn("UserFullName").AsString();
    }

    public override void Down()
    {
        Delete.Column("UserFullName").FromTable("Users");
    }
}