using TestWebTgBot.Models.Dbo;
using TestWebTgBot.Models.TgModels;
using TestWebTgBot.Services.User;
using TestWebTgBot.Utils;
using TestWebTgBot.Utils.Buttons;

namespace TestWebTgBot.TgBot.UpdateHandlers.User;

public class GoToStartButtonsHandler : IUpdateHandler
{
    private readonly TelegramBot _telegramBot;
    private readonly IUserService _userService;

    public GoToStartButtonsHandler(TelegramBot telegramBot, IUserService userService)
    {
        _telegramBot = telegramBot;
        _userService = userService;
    }
    public async Task HandleUpdateAsync(Update update)
    {
        var chatId = update.CallbackQuery!.From.Id;
        var messageId = update.CallbackQuery!.Message!.MessageId;
        
        var greetings = DateTime.Now.UtcToGreetingsString();
        
        var user = new UserEntity();
        user.Id = chatId;
        user = await _userService.CreateUserIfNotExistAsync(user);
        user = await _userService.SetStateDefault(user);
        
        var startPanelButtons = ButtonsHelper.ButtonsStartPanel(user.IsSubscribed);

        await _telegramBot.EditMessageText(chatId, messageId,$"{greetings}, добро пожаловат!", startPanelButtons);
    }

    public bool CanHandle(Update update)
    {
        if (update.CallbackQuery?.Message is not null && update.CallbackQuery?.From.Id is not null)
        {
            return update.CallbackQuery.Data == "StartMenu";
        }
        return false;
    }
}