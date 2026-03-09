using TestWebTgBot.Models.Dbo;

namespace TestWebTgBot.Repositories.Admin;

public interface IQuizAdminRepository
{
    Task<QuizAdminEntity> StartQuizAsync(QuizAdminEntity quizAdminEntity);

    Task<QuizAdminEntity> EndLastQuizAsync();

    Task<List<ResultQuizEntity>> GetResultsForLastTwoQuizzesAsync();

    Task<int?> GetLastQuizIdAsync();
    
    Task<bool> IsLastQuizActiveAsync();
}
