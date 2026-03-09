using System.Data;
using Dapper;
using TestWebTgBot.Models.Dbo;

namespace TestWebTgBot.Repositories.Admin;

public class QuizAdminRepository : IQuizAdminRepository
{
    private readonly IDbConnection _dbConnection;

    public QuizAdminRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }
    private DateTime GetMoscowTime()
    {
        var moscowTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Russian Standard Time");
        return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, moscowTimeZone);
    }

    public async Task<QuizAdminEntity> StartQuizAsync(QuizAdminEntity quizAdminEntity)
    {
        quizAdminEntity.StartTime = GetMoscowTime();

        var sql = "INSERT INTO QuizAdmin(IsStart, StartTime) VALUES(@IsStart, @StartTime)";
        await _dbConnection.ExecuteAsync(sql, quizAdminEntity);

        return quizAdminEntity;
    }

    public async Task<QuizAdminEntity?> EndLastQuizAsync()
    {
        var moscowTime = GetMoscowTime();

        const string getLastQuizSql = @"SELECT Id FROM QuizAdmin WHERE IsStart = 1 ORDER BY Id DESC LIMIT 1";
        var lastQuizId = await _dbConnection.QueryFirstOrDefaultAsync<int?>(getLastQuizSql);

        if (lastQuizId == null)
        {
            return null;
        }

        const string endQuizSql = @"UPDATE QuizAdmin SET IsStart = 0, EndTime = @EndTime WHERE Id = @Id";
        await _dbConnection.ExecuteAsync(endQuizSql, new { EndTime = moscowTime, Id = lastQuizId });

        return new QuizAdminEntity
        {
            Id = lastQuizId.Value,
            EndTime = moscowTime,
            IsStart = false
        };
    }
    public async Task<List<ResultQuizEntity>> GetResultsForLastTwoQuizzesAsync()
    {
        var quizIdSql = @"SELECT Id FROM QuizAdmin ORDER BY Id DESC LIMIT 2";

        var lastTwoQuizIds = await _dbConnection.QueryAsync<int>(quizIdSql);
        var quizIds = lastTwoQuizIds.ToList();

        if (!quizIds.Any())
        {
            return new List<ResultQuizEntity>();
        }
        
        var resultSql = @"
        SELECT uaq.Id, us.FullName AS UserFullName, uaq.TotalPoints
        FROM UserAnswersQuiz uaq
        INNER JOIN Users us ON uaq.UserId = us.Id
        WHERE uaq.QuizId IN @QuizIds";

        var results = await _dbConnection.QueryAsync<ResultQuizEntity>(resultSql, new { QuizIds = quizIds });
        return results.ToList();
    }
    
    public async Task<int?> GetLastQuizIdAsync()
    {
        var sql = @"SELECT Id FROM QuizAdmin ORDER BY Id DESC LIMIT 1";

        var lastQuizId = await _dbConnection.QueryFirstOrDefaultAsync<int?>(sql);
        return lastQuizId;
    }
    
    public async Task<bool> IsLastQuizActiveAsync()
    {
        const string sql = @"
                SELECT IsStart 
                FROM QuizAdmin 
                ORDER BY Id DESC 
                LIMIT 1";

        var isActive = await _dbConnection.QueryFirstOrDefaultAsync<bool?>(sql);

        return isActive ?? false;
    }
}
