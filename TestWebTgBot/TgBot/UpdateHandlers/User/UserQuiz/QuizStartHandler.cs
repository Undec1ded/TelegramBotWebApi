using TestWebTgBot.Models.Dbo;
using TestWebTgBot.Models.TgModels;
using TestWebTgBot.Services.User;
using TestWebTgBot.Utils;
using TestWebTgBot.Utils.Buttons;

namespace TestWebTgBot.TgBot.UpdateHandlers.User.UserQuiz;

public class QuizStartHandler : IUpdateHandler
{
    private readonly TelegramBot _telegramBot;
    private readonly IUserService _userService;

    public QuizStartHandler(TelegramBot telegramBot, IUserService userService)
    {
        _telegramBot = telegramBot;
        _userService = userService;
    }
    public async Task HandleUpdateAsync(Update update)
    {
        var chatId = update.CallbackQuery!.From.Id;
        var messageId = update.CallbackQuery!.Message!.MessageId;
        
        var buttonStartMenu = QuizButtonsHelper.GoToMainUserMenuButton();
        var quizContextButtons = QuizButtonsHelper.QuizStartContextButtons();

        var isQuizStart = await _userService.IsLastQuizActiveAsync();
        if (!isQuizStart)
        {
            await _telegramBot.EditMessageText(chatId, messageId, "Квиз еще не начат, пожалуйста подождите:з",
                buttonStartMenu);
            return;
        }
        
        await _telegramBot.EditMessageText(chatId, messageId, "Сыграть в квиз",
            quizContextButtons);
    }

    public bool CanHandle(Update update)
    {
        if (update.CallbackQuery?.Message is not null && update.CallbackQuery?.From.Id is not null)
        {
            return update.CallbackQuery.Data == "QuizStartUser";
        }
        return false;
    }
}