
using FluentMigrator;

namespace TestWebTgBot.Migration;

[Migration(202411210)]
public class CreateUserTable : FluentMigrator.Migration
{
    public override void Up()
    {
        Create.Table("Users")
            .WithColumn("Id").AsInt64().PrimaryKey()
            .WithColumn("IsAdmin").AsBoolean().NotNullable().WithDefaultValue(false);
    }

    public override void Down()
    {
        Delete.Table("Users");
    }
}