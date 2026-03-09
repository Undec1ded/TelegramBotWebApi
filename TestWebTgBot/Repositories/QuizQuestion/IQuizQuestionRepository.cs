using TestWebTgBot.Models.Dbo;

namespace TestWebTgBot.Repositories.QuizQuestion;

public interface IQuizQuestionRepository
{
    Task<List<QuizQuestionEntity>> GetQuestionsByQuizAsync(int quizId);

    Task<QuizQuestionEntity?> GetFirstQuestionForQuizAsync(int quizId);
    Task AddQuestionAsync(QuizQuestionEntity question);
    Task<QuizQuestionEntity?> GetQuestionByIdAsync(long variantQuestionId);

}
