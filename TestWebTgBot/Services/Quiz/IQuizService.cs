using TestWebTgBot.Models.Dbo;

namespace TestWebTgBot.Services.Quiz;

public interface IQuizService
{
    /// <summary>
    /// Adds a user's answer to a quiz question.
    /// </summary>
    /// <param name="userId">The ID of the user providing the answer.</param>
    /// <param name="questionId">The ID of the answered question.</param>
    /// <param name="answerId">The ID of the selected answer.</param>
    /// <param name="messageId">The ID of the message associated with the answer.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task AddUserAnswerAsync(long userId, long questionId, long answerId, int messageId);

    /// <summary>
    /// Starts a quiz for a user and associates it with a specific message.
    /// </summary>
    /// <param name="userId">The ID of the user who is starting the quiz.</param>
    /// <param name="messageId">The ID of the message associated with starting the quiz.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task StartQuizForUser(long userId, int messageId);

    Task ActivateQuizAsync(long? quizId);

    Task DeactivateQuizAsync();

    Task<bool> ActiveQuizExistAsync();

    Task AddQuestionToQuiz(QuizQuestionEntity question);

    Task AddVariantToQuestion(VariantEntity variant);

    Task RemoveVariantAsync(long variantId);
    Task<QuizEntity> GetCurrentQuiz();
    Task<bool> IsLastQuizActiveAsync();
    Task<QuizQuestionEntity> GetQuestionByIdAsync(long questionId);
    Task DeleteQuestionVariantAsync(long variantId);
}
