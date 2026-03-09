using System.Data;
using Dapper;
using TestWebTgBot.Models.Dbo;

namespace TestWebTgBot.Repositories.QuizAnswer;

public class QuizAnswerRepository : IQuizAnswerRepository
{
    private readonly IDbConnection _connection;

    public QuizAnswerRepository(IDbConnection connection)
    {
        _connection = connection;
    }

    public Task AddUserAnswerAsync(UserQuizAnswerEntity userAnswer)
    {
        var sql = @"
            INSERT INTO UserQuizAnswer (UserId, QuestionId, VariantId, MessageId)
            VALUES (@UserId, @QuestionId, @VariantId, @MessageId)";

        return _connection.ExecuteAsync(sql, userAnswer);
    }

    public async Task<List<UserQuizAnswerEntity>> GetUserQuizAnswers(long userId, int quizId)
    {
        var sql = @"
            SELECT *
            FROM UserQuizAnswer
            Join QuizQuestion q on q.Id = UserQuizAnswer.QuestionId
            WHERE UserId = @UserId AND QuizId = @QuizId;";

        return (await _connection.QueryAsync<UserQuizAnswerEntity>(sql, new { UserId = userId, QuizId = quizId })).ToList();
    }

    public Task ClearUserAnswersMessages(long userId, int quizId)
    {
        var sql = @"
            UPDATE UserQuizAnswer 
            Join QuizQuestion q on q.Id = UserQuizAnswer.QuestionId
            SET MessageId = NULL
            WHERE UserId = @UserId AND QuizId = @QuizId;";

        return _connection.ExecuteAsync(sql, new { UserId = userId, QuizId = quizId });
    }
}
