using TestWebTgBot.Models.Dbo;
using TestWebTgBot.Models.Internal;
using TestWebTgBot.Models.TgModels;
using TestWebTgBot.Services.User;
using TestWebTgBot.Utils;
using TestWebTgBot.Utils.Buttons;

namespace TestWebTgBot.TgBot.UpdateHandlers.Admin;

public class AdminMainHandler : IUpdateHandler
{
    private readonly TelegramBot _telegramBot;
    private readonly IUserService _userService;

    public AdminMainHandler(TelegramBot telegramBot, IUserService userService)
    {
        _telegramBot = telegramBot;
        _userService = userService;
    }
    public async Task HandleUpdateAsync(Update update)
    {
        var chatId = update.Message!.From!.Id;
        var adminMainButtons = AdminButtonsHelper.CrateAdminPanel();
        
        var user = new UserEntity();
        user.Id = chatId;
        user = await _userService.SetStateDefault(user);
        user = await _userService.GetUserByTelegramId(chatId);
        bool isAdmin = user!.IsAdmin;
        bool isSubscribed = user.IsSubscribed;
        var startPanelButtons = ButtonsHelper.ButtonsStartPanel(isSubscribed);
        
        if (!isAdmin)
        {
            await _telegramBot.SendTextMessage(chatId, "Недостаточно прав доступа");
            await _telegramBot.SendTextMessage(chatId, "QQ", startPanelButtons);
            return;
        }

        await _telegramBot.SendTextMessage(chatId, "Панель упарвления администратора", inlineReplyMarkup: adminMainButtons);
    }

    public bool CanHandle(Update update)
    {
        if (update.Message?.Text is not null &&
            update.Message.From is not null &&
            update.UserData.UserChatState == UserChatStates.Default)
        {
            return update.Message.Text == "/admin";
        }
        return false;
    }
}