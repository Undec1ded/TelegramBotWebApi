using TestWebTgBot.Models.Dbo;
using TestWebTgBot.Models.TgModels;
using TestWebTgBot.Services.Admin;
using TestWebTgBot.Services.User;
using TestWebTgBot.Utils;
using TestWebTgBot.Utils.Buttons;

namespace TestWebTgBot.TgBot.UpdateHandlers.Admin;

public class AdminUpdateHandler : IUpdateHandler
{
    private readonly TelegramBot _telegramBot;
    private readonly IUserService _userService;
    private readonly IAdminService _adminService;

    public AdminUpdateHandler(TelegramBot telegramBot, IUserService userService, IAdminService adminService)
    {
        _telegramBot = telegramBot;
        _userService = userService;
        _adminService = adminService;
    }

    public async Task HandleUpdateAsync(Update update)
    {
        var chatId = update.Message!.From!.Id;
        
        var adminButtons = AdminButtonsHelper.CrateAdminPanel();
        var buttonGoToStartMenu = ButtonsHelper.ButtonGoToStartUserButtons();
        
        var enteredPassword = update.Message.Text!.Split(' ').Last();

        var adminPasswords = await _adminService.GetAllAdminPasswordsAsync();
        var matchingPassword = adminPasswords.FirstOrDefault(p => p.Password == enteredPassword);

        if (matchingPassword == null)
        {
            await _telegramBot.SendTextMessage(chatId, "Ошибка: Неверный пароль, обратитесь к администратору.", buttonGoToStartMenu);
            return;
        }

        await _adminService.DeletePasswordAsync(enteredPassword);

        var user = new UserEntity
        {
            Id = chatId
        };
        user = await _userService.SetUserAdminAsync(user);

        await _telegramBot.SendTextMessage(chatId, 
            $"Пользователь {update.Message.From.Id} " +
            $"({(update.Message.From.UserName != null ? update.Message.From.UserName : update.Message.From.Id)}) " +
            $"добавлен в администраторы.");
        await _telegramBot.SendTextMessage(chatId, "Меню администратора", adminButtons);
    }

    public bool CanHandle(Update update)
    {
        if (update.Message?.Text is not null && update.Message.From is not null)
        {
            return update.Message.Text.StartsWith("/admin ");
        }
        return false;
    }
}
