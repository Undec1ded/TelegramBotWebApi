using System.Data;
using Dapper;
using TestWebTgBot.Models.Dbo;

namespace TestWebTgBot.Repositories.User;

public class QuestionsRepository : IQuestionsRepository
{
    private readonly IDbConnection _dbConnection;

    public QuestionsRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }
    public async Task<QuestionEntity> CreateQuestionAsync(QuestionEntity questionEntity)
    {
        var sql = "INSERT INTO Questions(Question, UserId) VALUES(@Question, @UserId)";
        var x = await _dbConnection.ExecuteAsync(sql, questionEntity);
        return questionEntity;
    }

    public async Task<QuestionEntity> DeleteQuestionsAsync(QuestionEntity questionEntity)
    {
        var sql = "DELETE FROM Questions";
        var x = await _dbConnection.ExecuteAsync(sql, questionEntity);
        return questionEntity;
    }

    public async Task<List<UserQuestionEntity>> GetQuestionsAsync()
    {
        var sql = @"SELECT Question, u.UserFullName
       FROM questions q
       INNER JOIN users u ON q.UserId = u.Id;";
        var questions = await _dbConnection.QueryAsync<UserQuestionEntity>(sql);
        return questions.ToList();
    }
}