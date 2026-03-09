using FluentMigrator;

namespace TestWebTgBot.Migration;

[Migration(202412030)]
public class AddColumnIsQuizStartIntoUsersTable : FluentMigrator.Migration
{
    public override void Up()
    {
        Alter.Table("Users").AddColumn("IsQuizStart").AsBoolean().NotNullable().WithDefaultValue(false);
    }

    public override void Down()
    {
        Delete.Column("IsQuizStart").FromTable("Users");
    }
}