using System.Data;
using Dapper;
using TestWebTgBot.Models.Dbo;

namespace TestWebTgBot.Repositories.User;

public class QuizUsersRepository : IQuizUsersRepository
{
    private readonly IDbConnection _dbConnection;

    public QuizUsersRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }


    public async Task<UserAnswersQuizEntity> CreateUserQuizAsync(UserAnswersQuizEntity userAnswersQuizEntity)
    {
        var sql = "INSERT INTO UserAnswersQuiz(QuizId, UserId) VALUES(@QuizId, @UserId)";
        var x = await _dbConnection.ExecuteAsync(sql, userAnswersQuizEntity);
        return userAnswersQuizEntity;
    }

    public async Task UpdateUserQuizDataAsync(UserAnswersQuizEntity userAnswersQuizEntity)
    {
        const string sql = @"
        UPDATE UserAnswersQuiz 
        SET 
            TotalPoints = COALESCE(@TotalPoints, TotalPoints),
            EndQuizMessageId = COALESCE(@EndQuizMessageId, EndQuizMessageId),
            Answer1 = COALESCE(@Answer1, Answer1),
            Question1MessageId = COALESCE(@Question1MessageId, Question1MessageId),
            Answer2 = COALESCE(@Answer2, Answer2),
            Question2MessageId = COALESCE(@Question2MessageId, Question2MessageId),
            Answer3 = COALESCE(@Answer3, Answer3),
            Question3MessageId = COALESCE(@Question3MessageId, Question3MessageId),
            Answer4 = COALESCE(@Answer4, Answer4),
            Question4MessageId = COALESCE(@Question4MessageId, Question4MessageId),
            Answer5 = COALESCE(@Answer5, Answer5),
            Question5MessageId = COALESCE(@Question5MessageId, Question5MessageId),
            Answer6 = COALESCE(@Answer6, Answer6),
            Question6MessageId = COALESCE(@Question6MessageId, Question6MessageId),
            Answer7 = COALESCE(@Answer7, Answer7),
            Question7MessageId = COALESCE(@Question7MessageId, Question7MessageId),
            Answer8 = COALESCE(@Answer8, Answer8),
            Question8MessageId = COALESCE(@Question8MessageId, Question8MessageId),
            Answer9 = COALESCE(@Answer9, Answer9),
            Question9MessageId = COALESCE(@Question9MessageId, Question9MessageId),
            Answer10 = COALESCE(@Answer10, Answer10),
            Question10MessageId = COALESCE(@Question10MessageId, Question10MessageId),
            Answer11 = COALESCE(@Answer11, Answer11),
            Question11MessageId = COALESCE(@Question11MessageId, Question11MessageId),
            Answer12 = COALESCE(@Answer12, Answer12),
            Question12MessageId = COALESCE(@Question12MessageId, Question12MessageId)
        WHERE QuizId = @QuizId AND UserId = @UserId;";

        await _dbConnection.ExecuteAsync(sql, userAnswersQuizEntity);
    }

    public async Task<bool> HasUserParticipatedInLastQuizAsync(long userId, int quizId)
    {
        const string sql = @"
        SELECT COUNT(1)
        FROM UserAnswersQuiz
        WHERE UserId = @UserId AND QuizId = @QuizId";

        var count = await _dbConnection.ExecuteScalarAsync<int>(sql, new { UserId = userId, QuizId = quizId });
        return count > 0;
    }

    public async Task<List<int>> GetQuestionMessageIdsAsync(int quizId, long userId)
    {
        var sql = @"
        SELECT 
            Question1MessageId, Question2MessageId, Question3MessageId, Question4MessageId, 
            Question5MessageId, Question6MessageId, Question7MessageId, Question8MessageId, 
            Question9MessageId, Question10MessageId, Question11MessageId, Question12MessageId
        FROM UserAnswersQuiz
        WHERE QuizId = @QuizId AND UserId = @UserId";

        var messageIds = await _dbConnection.QuerySingleOrDefaultAsync(sql, new { QuizId = quizId, UserId = userId });

        if (messageIds == null)
            return new List<int>();

        return ((IDictionary<string, object>)messageIds)
            .Values
            .OfType<int?>()
            .Where(id => id.HasValue)
            .Select(id => id.Value)
            .ToList();
    }

    public async Task<UserAnswersQuizEntity?> GetUserQuizDataAsync(int quizId, long userId)
    {
        var sql = @"
        SELECT 
            Id,
            QuizId,
            UserId,
            TotalPoints,
            EndQuizMessageId,
            Answer1,
            Question1MessageId,
            Answer2,
            Question2MessageId,
            Answer3,
            Question3MessageId,
            Answer4,
            Question4MessageId,
            Answer5,
            Question5MessageId,
            Answer6,
            Question6MessageId,
            Answer7,
            Question7MessageId,
            Answer8,
            Question8MessageId,
            Answer9,
            Question9MessageId,
            Answer10,
            Question10MessageId,
            Answer11,
            Question11MessageId,
            Answer12,
            Question12MessageId
        FROM UserAnswersQuiz
        WHERE QuizId = @QuizId AND UserId = @UserId";

        return await _dbConnection.QuerySingleOrDefaultAsync<UserAnswersQuizEntity>(
            sql, 
            new { QuizId = quizId, UserId = userId }
        );
    }
}