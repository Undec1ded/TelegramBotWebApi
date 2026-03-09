using TestWebTgBot.Models.Dbo;
using TestWebTgBot.Models.Internal;
using TestWebTgBot.Models.TgModels;
using TestWebTgBot.Services.User;
using TestWebTgBot.Utils;
using TestWebTgBot.Utils.Buttons;

namespace TestWebTgBot.TgBot.UpdateHandlers;

public class StartCommandUpdateHandler : IUpdateHandler
{
    private readonly TelegramBot _telegramBot;
    private readonly IUserService _userService;

    public StartCommandUpdateHandler(TelegramBot telegramBot, IUserService userService)
    {
        _telegramBot = telegramBot;
        _userService = userService;
    }

    public async Task HandleUpdateAsync(Update update)
    {
        var chatId = update.Message!.From!.Id;
        var greetings = DateTime.Now.UtcToGreetingsString();

        var existingUser = await _userService.GetUserByTelegramId(chatId);

        if (existingUser != null && existingUser.UserFullName != "Unknown")
        {
            var userButtons = ButtonsHelper.ButtonsStartPanel(existingUser.IsSubscribed);
            await _telegramBot.SendTextMessage(chatId, $"{greetings}, добро пожаловать!", userButtons);
            return;
        }
        var newUser = new UserEntity
        {
            Id = chatId,
            UserChatState = UserChatStates.WaitUserFullName
        };

        await _userService.CreateUserIfNotExistAsync(newUser);
        await _userService.SetStateUserFullName(newUser);
        
        await _telegramBot.SendTextMessage(chatId, $"{greetings}, пожалуйста, введите ФИО");
        return;

    }

    public bool CanHandle(Update update)
    {
        if (update.Message?.Text is not null &&
            update.Message.From is not null &&
            update.UserData.UserChatState == UserChatStates.Default)
        {
            return update.Message.Text == "/start";
        }

        return false;
    }
}