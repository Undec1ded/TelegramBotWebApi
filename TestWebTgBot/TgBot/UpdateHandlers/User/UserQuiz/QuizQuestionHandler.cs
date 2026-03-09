using TestWebTgBot.Models.TgModels;
using TestWebTgBot.Services.Quiz;

namespace TestWebTgBot.TgBot.UpdateHandlers.User.UserQuiz;

public class QuizQuestionHandler : IUpdateHandler
{
    private readonly IQuizService _quizService;

    public QuizQuestionHandler(
        IQuizService quizService)
    {
        _quizService = quizService;
    }

    public async Task HandleUpdateAsync(Update update)
    {
        // получаем данные из запроса
        var userId = update.UserData.Id;
        var messageId = update.CallbackQuery!.Message!.MessageId;
        var parametrs = update.CallbackQuery.Data!.Split("_");
        var answeredQuestionId = long.Parse(parametrs[1]);
        var answerId = long.Parse(parametrs[2]);

        //обрабатываем ответ пользователя
        await _quizService.AddUserAnswerAsync(userId, answeredQuestionId, answerId, messageId);
    }

    public bool CanHandle(Update update)
    {
        if (update.CallbackQuery?.Message is not null && update.CallbackQuery?.From.Id is not null)
        {
            return update.CallbackQuery.Data?.StartsWith("answ_") ?? false;
        }
        return false;
    }
}
