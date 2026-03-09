using TestWebTgBot.Models.Dbo;
using TestWebTgBot.Models.TgModels;
using TestWebTgBot.Services.Quiz;
using TestWebTgBot.Services.User;

namespace TestWebTgBot.TgBot.UpdateHandlers.Admin.QuizAdmin.ViewEdit.AddQuestion;

public class AddQuestionQuizHandler : IUpdateHandler
{
    private readonly TelegramBot _telegramBot;
    private readonly IQuizService _quizService;
    private readonly IUserService _userService;

    public AddQuestionQuizHandler(
        TelegramBot telegramBot,
        IQuizService quizService,
        IUserService userService)
    {
        _telegramBot = telegramBot;
        _quizService = quizService;
        _userService = userService;
    }

    public async Task HandleUpdateAsync(Update update)
    {
        var chatId = update.UserData.Id;
        var messageId = update.CallbackQuery!.Message!.MessageId;

        var message = "Введите вопрос:";

        await _userService.SetStateAdminQuestionEntering(new UserEntity(){Id = chatId});

        await _telegramBot.EditMessageText(
            chatId,
            messageId,
            message);
    }

    public bool CanHandle(Update update)
    {
        if (update.CallbackQuery?.Message is not null && update.CallbackQuery?.From.Id is not null)
        {
            return update.CallbackQuery.Data == "AddQuestion";
        }
        return false;
    }
}
