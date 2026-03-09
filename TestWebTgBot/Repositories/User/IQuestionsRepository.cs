using TestWebTgBot.Models.Dbo;

namespace TestWebTgBot.Repositories.User;

public interface IQuestionsRepository
{
    Task<QuestionEntity> CreateQuestionAsync(QuestionEntity questionEntity);

    Task<QuestionEntity> DeleteQuestionsAsync(QuestionEntity questionEntity);

    Task<List<UserQuestionEntity>> GetQuestionsAsync();
}