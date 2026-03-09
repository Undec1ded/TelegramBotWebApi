using TestWebTgBot.Models.Dbo;
using TestWebTgBot.Models.TgModels;
using TestWebTgBot.Services.User;
using TestWebTgBot.Utils.Buttons;

namespace TestWebTgBot.TgBot.UpdateHandlers.User.UserEvents;

public class UnsubscribeFromEventHandler : IUpdateHandler
{
    private readonly TelegramBot _telegramBot;
    private readonly IUserService _userService;

    public UnsubscribeFromEventHandler(TelegramBot telegramBot, IUserService userService)
    {
        _telegramBot = telegramBot;
        _userService = userService;
    }

    public async Task HandleUpdateAsync(Update update)
    {
        var chatId = update.CallbackQuery!.From.Id;
        var messageId = update.CallbackQuery!.Message!.MessageId;
        var callbackData = update.CallbackQuery.Data;

        var buttonsStart = ButtonsHelper.ButtonGoToStartUserButtons();

        if (!int.TryParse(callbackData!.Split('_')[1], out var eventId))
        {
            await _telegramBot.EditMessageText(chatId, messageId, "Ошибка: ID мероприятия недействителен.", buttonsStart);
            return;
        }

        var allEvents = await _userService.GetAllEventsAsync();

        var eventInfo = allEvents.FirstOrDefault(e => e.Id == eventId);

        if (eventInfo == null)
        {
            await _telegramBot.EditMessageText(chatId, messageId, "Ошибка: мероприятие не найдено.", buttonsStart);
            return;
        }

        var success = await _userService.UnregisterUserFromEventAsync(chatId, eventId);

        if (success)
        {
            await _telegramBot.EditMessageText(chatId, messageId, 
                $"Вы успешно отписались от мероприятия: {eventInfo.NameEvent}.", 
                buttonsStart);
        }
        else
        {
            await _telegramBot.EditMessageText(chatId, messageId, 
                $"Ошибка: не удалось отписаться от мероприятия: {eventInfo.NameEvent}. Возможно, вы не были зарегистрированы.", 
                buttonsStart);
        }
    }


    public bool CanHandle(Update update)
    {
        if (update.CallbackQuery?.Message is not null && update.CallbackQuery?.From.Id is not null)
        {
            return update.CallbackQuery.Data?.StartsWith("UnregisterEvent_") ?? false;
        }

        return false;
    }
}