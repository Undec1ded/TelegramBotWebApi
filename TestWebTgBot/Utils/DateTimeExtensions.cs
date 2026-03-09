namespace TestWebTgBot.Utils;

public static class DateTimeExtensions
{
    public static string UtcToGreetingsString(this DateTime dateTime)
    {
        var utcTime = DateTime.UtcNow;
        var mskZone = TimeZoneInfo.FindSystemTimeZoneById("Europe/Moscow");

        var mskTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, mskZone);

        if (mskTime.Hour >= 0 && mskTime.Hour < 6)
        {
            return "Доброй ночи";
        }
        if (mskTime.Hour >= 6 && mskTime.Hour < 12)
        {
            return "Доброе утро";
        }
        if (mskTime.Hour >= 12 && mskTime.Hour < 18)
        {
            return "Добрый день";
        }
        return "Добрый вечер";
    }

    public static DateTime ToMoscowTime(this DateTime dateTime)
    {
        var moscowTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Russian Standard Time");
        return TimeZoneInfo.ConvertTimeFromUtc(dateTime.ToUniversalTime(), moscowTimeZone);
    }
}
