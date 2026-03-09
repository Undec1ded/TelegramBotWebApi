using FluentMigrator;

namespace TestWebTgBot.Migration;

[Migration(202412071)]
public class CreateTableAdminPasswords : FluentMigrator.Migration
{
    public override void Up()
    {
        Create.Table("AdminPasswords")
            .WithColumn("Id").AsInt32().PrimaryKey().Identity()
            .WithColumn("Password").AsString().NotNullable()
            .WithColumn("TimeCreated").AsDateTime().NotNullable();
    }

    public override void Down()
    {
        Delete.Table("AdminPasswords");
    }
}