using FluentMigrator;

namespace TestWebTgBot.Migration;

[Migration(202411300)]
public class CreateLinksSocialNetworksTable : FluentMigrator.Migration
{
    public override void Up()
    {
        Create.Table("LinksSocialNetworks")
            .WithColumn("Id").AsInt32().PrimaryKey().Identity()
            .WithColumn("Link").AsString().NotNullable();
    }

    public override void Down()
    {
        Delete.Table("LinksSocialNetworks");
    }
}