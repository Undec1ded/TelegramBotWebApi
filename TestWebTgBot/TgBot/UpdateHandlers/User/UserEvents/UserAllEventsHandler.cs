using TestWebTgBot.Models.TgModels;
using TestWebTgBot.Services.User;
using TestWebTgBot.Utils.Buttons;

namespace TestWebTgBot.TgBot.UpdateHandlers.User.UserEvents;

public class UserAllEventsHandler : IUpdateHandler
{
    private readonly TelegramBot _telegramBot;
    private readonly IUserService _userService;

    public UserAllEventsHandler(TelegramBot telegramBot, IUserService userService)
    {
        _telegramBot = telegramBot;
        _userService = userService;
    }

    public async Task HandleUpdateAsync(Update update)
    {
        var chatId = update.CallbackQuery!.From.Id;
        var messageId = update.CallbackQuery!.Message!.MessageId;

        var buttonsContext = ButtonsHelper.ButtonsEventsContext();

        var currentDateTime =  TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Europe/Moscow");

        var allEvents = await _userService.GetAllEventsAsync();

        var upcomingEvents = allEvents
            .Where(e => e.DateTimeEvent > currentDateTime)
            .OrderBy(e => e.DateTimeEvent)
            .ToList();

        if (!upcomingEvents.Any())
        {
            await _telegramBot.EditMessageText(
                chatId: chatId,
                messageId: messageId,
                text: "Нет предстоящих мероприятий."
            );
            return;
        }

        string eventsMessage = "Предстоящие мероприятия:\n\n";
        foreach (var ev in upcomingEvents)
        {
            eventsMessage += $"•{ev.DateTimeEvent:dd.MM.yyyy HH:mm} - {ev.NameEvent}\n";
        }
                                         
        var eventButtons = ButtonsHelper.CreateEventButtons(upcomingEvents);
        await _telegramBot.EditMessageText(chatId, messageId, $"{eventsMessage}\nЗаписаться на мероприятие:", eventButtons);
    }

    public bool CanHandle(Update update)
    {
        if (update.CallbackQuery?.Message is not null && update.CallbackQuery?.From.Id is not null)
        {
            return update.CallbackQuery.Data == "ScheduleOfEvents";
        }
        return false;
    }
}