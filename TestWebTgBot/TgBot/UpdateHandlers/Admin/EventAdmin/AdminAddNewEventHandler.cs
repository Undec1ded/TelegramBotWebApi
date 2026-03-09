using TestWebTgBot.Models.Dbo;
using TestWebTgBot.Models.TgModels;
using TestWebTgBot.Services.Admin;
using TestWebTgBot.Utils.Buttons;

namespace TestWebTgBot.TgBot.UpdateHandlers.Admin.EventAdmin;

public class AdminAddNewEventHandler : IUpdateHandler
{
    private readonly TelegramBot _telegramBot;
    private readonly IAdminService _adminService;

    public AdminAddNewEventHandler(TelegramBot telegramBot, IAdminService adminService)
    {
        _telegramBot = telegramBot;
        _adminService = adminService;
    }
    public async Task HandleUpdateAsync(Update update)
    {
        var chatId = update.CallbackQuery!.From.Id;
        var messageId = update.CallbackQuery.Message!.MessageId;

        var buttonAdminStartPanel = AdminButtonsHelper.ButtonGoToAdminStartPanel();
        
        var user = new UserEntity();
        user.Id = chatId;
        user = await _adminService.SetStateWaitNewEventAsync(user);

        await _telegramBot.EditMessageText(chatId, messageId, "Введите мероприятите по формату *дд.мм.гггг-чч.мм-Название мероприятия",
            buttonAdminStartPanel);
    }

    public bool CanHandle(Update update)
    {
        if (update.CallbackQuery?.Message is not null && update.CallbackQuery?.From.Id is not null)
        {
            return update.CallbackQuery.Data == "AdminAddEvent";
        }
        return false;
    }
}