using System.Data;
using Dapper;
using TestWebTgBot.Models.Dbo;

namespace TestWebTgBot.Repositories.Quiz;

public class QuizRepository : IQuizRepository
{
    private readonly IDbConnection _connection;

    public QuizRepository(IDbConnection connection)
    {
        _connection = connection;
    }

    public async Task<QuizEntity> CreateQuizAsync()
    {
        var sql = @"
            INSERT INTO Quiz (Active, StartTime, EndTime)
            VALUES (0, NULL, NULL); 
            SELECT LAST_INSERT_ID();";
        var id = await _connection.ExecuteScalarAsync<int>(sql);
        var quiz = new QuizEntity
        {
            Id = id,
            Active = false,
            StartTime = null,
            EndTime = null,
            Questions = null
        };
        return quiz;
    }

    public async Task<QuizEntity?> GetActiveQuizDetailedAsync()
    {
        var sql = @"
        SELECT q.Id, q.Active, q.StartTime, q.EndTime,
               que.Id, que.QuizId, que.Question,
               v.Id, v.Text, v.IsCorrect, v.QuestionId
        FROM Quiz q
                 LEFT JOIN QuizQuestion que ON que.QuizId = q.Id
                 LEFT JOIN QuizVariant v ON v.QuestionId = que.Id
        WHERE q.Id = (SELECT MAX(Id) FROM Quiz WHERE Active = 1)
        ORDER BY q.Id DESC
    ";

        var quizDictionary = new Dictionary<int, QuizEntity>();

        await _connection.QueryAsync<QuizEntity, QuizQuestionEntity, VariantEntity, QuizEntity>(
            sql,
            (quiz, question, variant) =>
            {
                if (!quizDictionary.TryGetValue(quiz.Id, out var quizEntry))
                {
                    quizEntry = quiz;
                    quizEntry.Questions = new List<QuizQuestionEntity>();
                    quizDictionary[quiz.Id] = quizEntry;
                }

                if (question != null)
                {
                    var questionEntry = quizEntry.Questions.FirstOrDefault(q => q.Id == question.Id);
                    if (questionEntry == null)
                    {
                        questionEntry = question;
                        questionEntry.Variants = new List<VariantEntity>();
                        quizEntry.Questions.Add(questionEntry);
                    }

                    if (variant != null)
                    {
                        questionEntry.Variants.Add(variant);
                    }
                }

                return quizEntry;
            },
            splitOn: "Id,Id"
        );

        return quizDictionary.Values.FirstOrDefault();
    }

    public Task<QuizEntity?> GetQuizByIdAsync(long quizId)
    {
        var sql = @"
            SELECT *
            FROM Quiz
            WHERE Id = @QuizId;";
        return _connection.QueryFirstOrDefaultAsync<QuizEntity>(sql, new { QuizId = quizId });
    }

    public async Task<QuizEntity?> GetLastQuizDetailedAsync()
    {
        var sql = @"
        SELECT q.Id, q.Active, q.StartTime, q.EndTime,
               que.Id, que.QuizId, que.Question,
               v.Id, v.Text, v.IsCorrect, v.QuestionId
        FROM Quiz q
                 LEFT JOIN QuizQuestion que ON que.QuizId = q.Id
                 LEFT JOIN QuizVariant v ON v.QuestionId = que.Id
        WHERE q.Id = (SELECT MAX(Id) FROM Quiz)
        ORDER BY q.Id DESC
    ";

        var quizDictionary = new Dictionary<int, QuizEntity>();

        await _connection.QueryAsync<QuizEntity, QuizQuestionEntity, VariantEntity, QuizEntity>(
            sql,
            (quiz, question, variant) =>
            {
                if (!quizDictionary.TryGetValue(quiz.Id, out var quizEntry))
                {
                    quizEntry = quiz;
                    quizEntry.Questions = new List<QuizQuestionEntity>();
                    quizDictionary[quiz.Id] = quizEntry;
                }

                if (question != null)
                {
                    var questionEntry = quizEntry.Questions.FirstOrDefault(q => q.Id == question.Id);
                    if (questionEntry == null)
                    {
                        questionEntry = question;
                        questionEntry.Variants = new List<VariantEntity>();
                        quizEntry.Questions.Add(questionEntry);
                    }

                    if (variant != null)
                    {
                        questionEntry.Variants.Add(variant);
                    }
                }

                return quizEntry;
            },
            splitOn: "Id,Id"
        );

        return quizDictionary.Values.FirstOrDefault();
    }

    public Task ActivateQuizAsync(long quizId, DateTime toMoscowTime)
    {
        var sql = @"
            UPDATE Quiz
            SET Active = 1, StartTime = @StartTime
            WHERE Id = @QuizId;";
        return _connection.ExecuteAsync(sql, new { QuizId = quizId, StartTime = toMoscowTime });
    }

    public Task DeactivateAllQuizzesAsync(DateTime toMoscowTime)
    {
        var sql = @"
            UPDATE Quiz
            SET Active = 0, EndTime = @EndTime
            WHERE Active = 1;";
        return _connection.ExecuteAsync(sql, new { EndTime = toMoscowTime });
    }

    public Task<bool> IsLastQuizActiveAsync()
    {
        var sql = @"
            SELECT Active
            FROM Quiz
            ORDER BY Id DESC
            LIMIT 1;";
        return _connection.ExecuteScalarAsync<bool>(sql);
    }
}
