using FluentMigrator;

namespace TestWebTgBot.Migration;
[Migration(202411301)]
public class AddColumnLinkName : FluentMigrator.Migration
{
    public override void Up()
    {
        Alter.Table("LinksSocialNetworks").AddColumn("LinkName").AsString().NotNullable();
    }

    public override void Down()
    {
        Delete.Column("LinkName").FromTable("LinksSocialNetworks");
    }
}