using TestWebTgBot.Models.TgModels;
using TestWebTgBot.Services.Quiz;
using TestWebTgBot.Utils.Buttons;

namespace TestWebTgBot.TgBot.UpdateHandlers.User.UserQuiz;

public class QuizFirstQuestionHandler : IUpdateHandler
{
    private readonly IQuizService _quizService;

    public QuizFirstQuestionHandler(
        IQuizService quizService)
    {
        _quizService = quizService;
    }

    public async Task HandleUpdateAsync(Update update)
    {
        // получаем данные из запроса
        var userId = update.UserData.Id;
        var messageId = update.CallbackQuery!.Message!.MessageId;

        // начинаем квиз
        await _quizService.StartQuizForUser(userId, messageId);
    }

    public bool CanHandle(Update update)
    {
        if (update.CallbackQuery?.Message is not null && update.CallbackQuery?.From.Id is not null)
        {
            return update.CallbackQuery.Data == "UserStartQuizNow";
        }
        return false;
    }
}
