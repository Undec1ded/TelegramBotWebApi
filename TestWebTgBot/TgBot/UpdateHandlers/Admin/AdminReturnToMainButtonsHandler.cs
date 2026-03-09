using TestWebTgBot.Models.Dbo;
using TestWebTgBot.Models.TgModels;
using TestWebTgBot.Services.Admin;
using TestWebTgBot.Services.User;
using TestWebTgBot.Utils.Buttons;

namespace TestWebTgBot.TgBot.UpdateHandlers.Admin;

public class AdminReturnToMainButtonsHandler : IUpdateHandler
{
    private readonly TelegramBot _telegramBot;
    private readonly IAdminService _adminService;

    public AdminReturnToMainButtonsHandler(TelegramBot telegramBot, IAdminService adminService)
    {
        _telegramBot = telegramBot;
        _adminService = adminService;
    }
    public async Task HandleUpdateAsync(Update update)
    {
        long chatId = update.CallbackQuery!.From.Id;
        int messageId = update.CallbackQuery!.Message!.MessageId;
        
        var buttonsAdminMain = AdminButtonsHelper.CrateAdminPanel();

        var user = new UserEntity();
        user.Id = chatId;

        user = await _adminService.SetStateDefaultAsync(user); 
        
        await _telegramBot.EditMessageText(chatId, messageId, "Панель Администратора", buttonsAdminMain);
    }

    public bool CanHandle(Update update)
    {
        if (update.CallbackQuery?.Message is not null && update.CallbackQuery?.From.Id is not null)
        {
            return update.CallbackQuery.Data == "GoToAdminStartPanel";
        }
        return false;
    }
}