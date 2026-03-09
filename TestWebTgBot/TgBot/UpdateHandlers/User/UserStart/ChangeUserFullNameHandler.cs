using TestWebTgBot.Models.Dbo;
using TestWebTgBot.Models.Internal;
using TestWebTgBot.Models.TgModels;
using TestWebTgBot.Services.User;
using TestWebTgBot.Utils;

namespace TestWebTgBot.TgBot.UpdateHandlers.User.UserStart;

public class ChangeUserFullNameHandler : IUpdateHandler
{
    private readonly TelegramBot _telegramBot;
    private readonly IUserService _userService;

    public ChangeUserFullNameHandler(TelegramBot telegramBot , IUserService userService)
    {
        _telegramBot = telegramBot;
        _userService = userService;
    }

    public async Task HandleUpdateAsync(Update update)
    {
        var chatId = update.CallbackQuery!.From.Id;
        var greetings = DateTime.Now.UtcToGreetingsString();

        var user = new UserEntity();
        user.Id = chatId;
        user = await _userService.SetStateUserFullName(user);
        
        await _telegramBot.SendTextMessage(chatId, $"{greetings}, пожалуйста, введите ФИО");
    }

    public bool CanHandle(Update update)
    {
        if (update.CallbackQuery?.Message is not null && update.CallbackQuery?.From.Id is not null)
        {
            return update.CallbackQuery.Data == "ChangeUserFullName";
        }
        return false;
    }
}