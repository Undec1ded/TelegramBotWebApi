using TestWebTgBot.Models.Dbo;

namespace TestWebTgBot.Repositories.QuizAnswer;

public interface IQuizAnswerRepository
{
    Task AddUserAnswerAsync(UserQuizAnswerEntity userAnswer);
    Task<List<UserQuizAnswerEntity>> GetUserQuizAnswers(long userId, int quizId);
    Task ClearUserAnswersMessages(long userId, int quizId);
}
