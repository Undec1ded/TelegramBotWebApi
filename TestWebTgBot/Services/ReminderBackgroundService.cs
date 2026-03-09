using TestWebTgBot.Services.User;
using TestWebTgBot.TgBot;

namespace TestWebTgBot.Services;

public class ReminderBackgroundService : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly TimeSpan _reminderInterval = TimeSpan.FromMinutes(10); 
    private readonly TimeSpan _reminderTime = TimeSpan.FromHours(1); 

    public ReminderBackgroundService(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _serviceScopeFactory.CreateScope();

                var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
                var telegramBot = scope.ServiceProvider.GetRequiredService<TelegramBot>();

                var nowMoscowTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Europe/Moscow");

                var events = await userService.GetAllEventsAsync();

                var upcomingEvents = events
                    .Where(e => e.DateTimeEvent > nowMoscowTime && e.DateTimeEvent <= nowMoscowTime.Add(_reminderTime))
                    .ToList();

                foreach (var upcomingEvent in upcomingEvents)
                {
                    var registeredUsers = await userService.GetUsersByEventIdAsync(upcomingEvent.Id);
                    await userService.SetIsNotifiedAsync(upcomingEvent.Id);
                    foreach (var user in registeredUsers)
                    {
                        var message = $"Напоминаем! Мероприятие \"{upcomingEvent.NameEvent}\" начнется {upcomingEvent.DateTimeEvent:dd.MM.yyyy HH:mm}.";
                        await telegramBot.SendTextMessage(user.Id, message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка в ReminderBackgroundService: {ex.Message}");
            }

            await Task.Delay(_reminderInterval, stoppingToken);
        }
    }
}