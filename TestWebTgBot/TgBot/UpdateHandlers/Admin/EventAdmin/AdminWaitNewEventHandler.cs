using TestWebTgBot.Models.Dbo;
using TestWebTgBot.Models.Internal;
using TestWebTgBot.Models.TgModels;
using TestWebTgBot.Services.Admin;
using TestWebTgBot.Utils.Buttons;

namespace TestWebTgBot.TgBot.UpdateHandlers.Admin.EventAdmin;

public class AdminWaitNewEventHandler : IUpdateHandler
{
    private readonly TelegramBot _telegramBot;
    private readonly IAdminService _adminService;

    public AdminWaitNewEventHandler(TelegramBot telegramBot, IAdminService adminService)
    {
        _telegramBot = telegramBot;
        _adminService = adminService;
    }
    public async Task HandleUpdateAsync(Update update)
    {
        var chatId = update.Message!.From!.Id;
        var message = update.Message.Text;

        var buttonsContext = AdminButtonsHelper.ButtonsAdminAddEventContext();
        
        var user = new UserEntity();
        user.Id = chatId;
        user = await _adminService.SetStateDefaultAsync(user);
        
        var parts = message!.Split('-');
        if (parts.Length < 3)
        {
            await _telegramBot.SendTextMessage(chatId, "Ошибка! Убедитесь, что дата и время указаны в формате дд.ММ.гггг-чч.мм", buttonsContext);
            return;  
        }

        var datePart = parts[0];
        var timePart = parts[1];
        var eventName = string.Join("-", parts.Skip(2));

        var dateTimeString = $"{datePart} {timePart}";

        if (!DateTime.TryParseExact(
                dateTimeString,
                "dd.MM.yyyy HH.mm",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None,
                out var eventDateTime))
        {
            await _telegramBot.SendTextMessage(chatId, "Ошибка! Убедитесь, что дата и время указаны в формате дд.мм.гггг-чч.мм", buttonsContext);
            return;
        }

        var newEvent = new ScheduleOfEventsEntity();
        newEvent.NameEvent = eventName;
        newEvent.DateTimeEvent = eventDateTime;
        newEvent = await _adminService.AddNewEventAsync(newEvent);
        
        await _telegramBot.SendTextMessage(chatId, "Мероприятие добавлено", buttonsContext);
        
    }

    public bool CanHandle(Update update)
    {
        if (update.Message?.Text is not null &&
            update.Message.From is not null &&
            update.UserData.UserChatState == UserChatStates.WaitAdminNewEvent)
        {
            return true;
        }
        return false;
    }
}