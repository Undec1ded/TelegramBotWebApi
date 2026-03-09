using TestWebTgBot.Models.Dbo;

namespace TestWebTgBot.Repositories.Quiz;

public interface IQuizRepository
{
    Task<QuizEntity> CreateQuizAsync();

    Task<QuizEntity?> GetActiveQuizDetailedAsync();
    Task<QuizEntity?> GetQuizByIdAsync(long quizId);
    Task<QuizEntity?> GetLastQuizDetailedAsync();
    Task ActivateQuizAsync(long quizId, DateTime toMoscowTime);
    Task DeactivateAllQuizzesAsync(DateTime toMoscowTime);
    Task<bool> IsLastQuizActiveAsync();
}
