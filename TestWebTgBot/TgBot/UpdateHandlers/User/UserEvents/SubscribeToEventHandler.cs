using TestWebTgBot.Models.Dbo;
using TestWebTgBot.Models.TgModels;
using TestWebTgBot.Services.User;
using TestWebTgBot.Utils.Buttons;

namespace TestWebTgBot.TgBot.UpdateHandlers.User.UserEvents;

public class SubscribeToEventHandler : IUpdateHandler
{
    private readonly TelegramBot _telegramBot;
    private readonly IUserService _userService;

    public SubscribeToEventHandler(TelegramBot telegramBot, IUserService userService)
    {
        _telegramBot = telegramBot;
        _userService = userService;
    }

    public async Task HandleUpdateAsync(Update update)
    {
        var chatId = update.CallbackQuery!.From.Id;
        var callbackData = update.CallbackQuery.Data!;
        var messageId = update.CallbackQuery!.Message!.MessageId;

        var buttonContext = ButtonsHelper.ButtonsEventsAddContext();

        var eventId = int.Parse(callbackData.Split('_')[1]);
        
        var allRegisteredEvents = await _userService.GetAllRegisteredEventsAsync();
        var isAlreadyRegistered = allRegisteredEvents.Any(e => e.UserId == chatId && e.IdEvent == eventId);

        if (isAlreadyRegistered)
        {
            await _telegramBot.EditMessageText(chatId, messageId,"Вы уже записаны на это мероприятие!", buttonContext);
            return;
        }

        var userEvents = new UserEventsEntity
        {
            UserId = chatId,
            IdEvent = eventId
        };

        userEvents = await _userService.RegisterUserForEventAsync(userEvents);

        var allEvents = await _userService.GetAllEventsAsync();
        var selectedEvent = allEvents.FirstOrDefault(e => e.Id == eventId);

        var eventName = selectedEvent?.NameEvent ?? "Мероприятие";
        var eventDate = selectedEvent?.DateTimeEvent.ToString("dd.MM.yyyy HH:mm") ?? "неизвестно";

        await _telegramBot.EditMessageText(
            chatId,
            messageId, 
            $"Вы успешно записались на мероприятие: \"{eventName}\".\nДата и время: {eventDate}",
            buttonContext
        );
    }


    public bool CanHandle(Update update)
    {
        if (update.CallbackQuery?.Message is not null && update.CallbackQuery?.From.Id is not null)
        {
            return update.CallbackQuery?.Data?.StartsWith("RegisterEvent_") ?? false;
        }
        return false;
    }   
}