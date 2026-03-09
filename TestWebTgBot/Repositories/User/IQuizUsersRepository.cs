using TestWebTgBot.Models.Dbo;

namespace TestWebTgBot.Repositories.User;

public interface IQuizUsersRepository
{
    Task<UserAnswersQuizEntity> CreateUserQuizAsync(UserAnswersQuizEntity userAnswersQuizEntity);

    Task UpdateUserQuizDataAsync(UserAnswersQuizEntity userAnswersQuizEntity);
    
    Task<bool> HasUserParticipatedInLastQuizAsync(long userId, int quizId);
    
    Task<List<int>> GetQuestionMessageIdsAsync(int quizId, long userId);
    
    Task<UserAnswersQuizEntity?> GetUserQuizDataAsync(int quizId, long userId);
}