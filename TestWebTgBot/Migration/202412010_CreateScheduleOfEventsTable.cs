using FluentMigrator;

namespace TestWebTgBot.Migration;

[Migration(202412010)]
public class CreateScheduleOfEventsTable : FluentMigrator.Migration
{
    public override void Up()
    {
        Create.Table("ScheduleOfEvents")
            .WithColumn("Id").AsInt32().PrimaryKey().Identity()
            .WithColumn("NameEvent").AsString().NotNullable()
            .WithColumn("DateTimeEvent").AsDateTime().NotNullable();
    }

    public override void Down()
    {
        Delete.Table("ScheduleOfEvents");
    }
}