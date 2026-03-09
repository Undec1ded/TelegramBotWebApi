using TestWebTgBot.Models.Dbo;
using TestWebTgBot.Models.TgModels;
using TestWebTgBot.Services.User;
using TestWebTgBot.Utils.Buttons;

namespace TestWebTgBot.TgBot.UpdateHandlers.User.UserQuestions;

public class UserTryAskQuestionHandler: IUpdateHandler
{
    private readonly TelegramBot _telegramBot;
    private readonly IUserService _userService;

    public UserTryAskQuestionHandler(TelegramBot telegramBot, IUserService userService)
    {
        _telegramBot = telegramBot;
        _userService = userService;
    }

    public async Task HandleUpdateAsync(Update update)
    {
        var chatId = update.CallbackQuery!.From.Id;
        var messageId = update.CallbackQuery!.Message!.MessageId;
        var buttonMainMenu = ButtonsHelper.ButtonGoToStartUserButtons();
        
        var user = new UserEntity();
        user.Id = chatId;
        user = await _userService.SetStateWaitUserQuestion(user);
        
        await _telegramBot.EditMessageText(chatId, messageId,"Напишите вопрос спикеру:", buttonMainMenu);
    }

    public bool CanHandle(Update update)
    {
        if (update.CallbackQuery?.Message is not null && update.CallbackQuery?.From.Id is not null)
        {
            return update.CallbackQuery.Data == "AskQuestion";
        }
        return false;
    }
}