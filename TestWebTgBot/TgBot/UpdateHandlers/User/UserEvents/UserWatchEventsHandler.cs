using TestWebTgBot.Models.Dbo;
using TestWebTgBot.Models.TgModels;
using TestWebTgBot.Services.User;
using TestWebTgBot.Utils.Buttons;

namespace TestWebTgBot.TgBot.UpdateHandlers.User.UserEvents;

public class UserWatchEventsHandler : IUpdateHandler
{
    private readonly TelegramBot _telegramBot;
    private readonly IUserService _userService;

    public UserWatchEventsHandler(TelegramBot telegramBot, IUserService userService)
    {
        _telegramBot = telegramBot;
        _userService = userService;
    }

    public async Task HandleUpdateAsync(Update update)
    {
        var chatId = update.CallbackQuery!.From.Id;
        var messageId = update.CallbackQuery!.Message!.MessageId;

        var buttonStart = ButtonsHelper.ButtonGoToStartUserButtons();

        var registeredEvents = await _userService.GetUserRegisteredEventsAsync(chatId);

        if (!registeredEvents.Any())
        {
            await _telegramBot.EditMessageText(chatId, messageId,"Вы не записаны ни на одно мероприятие.", buttonStart);
            return;
        }

        string message = "Вы записаны на мероприятие(я):\n";
        message += string.Join("\n", registeredEvents.Select(ev =>
            $"{ev.DateTimeEvent:dd.MM.yyyy HH:mm} - {ev.NameEvent}"));
        message += "\n\nОтписаться от:";

        var deleteButtons = ButtonsHelper.CreateEventDeleteButtons(registeredEvents);

        await _telegramBot.EditMessageText(chatId, messageId, message, deleteButtons);
    }

    public bool CanHandle(Update update)
    {
        if (update.CallbackQuery?.Message is not null && update.CallbackQuery?.From.Id is not null)
        {
            return update.CallbackQuery.Data == "RegistrationForEvents";
        }
        return false;
    }
}