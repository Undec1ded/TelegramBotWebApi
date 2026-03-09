using System.Data;
using Dapper;
using TestWebTgBot.Models.Dbo;

namespace TestWebTgBot.Repositories.QuizQuestion;

public class QuizQuestionRepository : IQuizQuestionRepository
{
    private readonly IDbConnection _connection;

    public QuizQuestionRepository(IDbConnection connection)
    {
        _connection = connection;
    }

    public async Task<List<QuizQuestionEntity>> GetQuestionsByQuizAsync(int quizId)
    {
        var sql = @"
        SELECT 
            q.*, 
            v.*
        FROM QuizQuestion q
        LEFT JOIN QuizVariant v ON q.Id = v.QuestionId
        WHERE q.QuizId = @QuizId";

        var questionDictionary = new Dictionary<long, QuizQuestionEntity>();

        var result = await _connection.QueryAsync<QuizQuestionEntity, VariantEntity, QuizQuestionEntity>(
            sql,
            (question, variant) =>
            {
                if (!questionDictionary.TryGetValue(question.Id, out var questionEntry))
                {
                    questionEntry = question;
                    questionEntry.Variants = new List<VariantEntity>();
                    questionDictionary[question.Id] = questionEntry;
                }
                if (variant != null)
                {
                    questionEntry.Variants.Add(variant);
                }
                return questionEntry;
            },
            new { QuizId = quizId },
            splitOn: "Id");

        return questionDictionary.Values.OrderBy(q=>q.Id).ToList();
    }

    public async Task<QuizQuestionEntity?> GetFirstQuestionForQuizAsync(int quizId)
    {
        var questionSql = @"
            SELECT *
            FROM
                QuizQuestion
            WHERE
                QuizId = @QuizId
            ORDER BY Id
            LIMIT 1";

        var variantSql = @"
            SELECT *
            FROM
                QuizVariant
            WHERE
                QuestionId = @QuestionId";

        var question = await _connection.QueryFirstOrDefaultAsync<QuizQuestionEntity>(questionSql, new { QuizId = quizId });
        var variants = await _connection.QueryAsync<VariantEntity>(variantSql, new { QuestionId = question?.Id });

        if (question is not null)
        {
            question.Variants = variants.ToList();
        }

        return question;
    }

    public Task AddQuestionAsync(QuizQuestionEntity question)
    {

        var sql = @"
            INSERT INTO QuizQuestion (QuizId, Question, ImagesIds)
            VALUES (@QuizId, @Question, @ImagesIds)";
        return _connection.ExecuteAsync(sql, question);
    }

    public async Task<QuizQuestionEntity?> GetQuestionByIdAsync(long variantQuestionId)
    {
        var questionSql = @"
            SELECT *
            FROM
                QuizQuestion
            WHERE
                Id = @Id";

        var variantSql = @"
            SELECT *
            FROM
                QuizVariant
            WHERE
                QuestionId = @Id";

        var question = await _connection.QueryFirstOrDefaultAsync<QuizQuestionEntity>(questionSql, new { Id = variantQuestionId });
        var variants = await _connection.QueryAsync<VariantEntity>(variantSql, new { Id = variantQuestionId });

        if (question is not null)
        {
            question.Variants = variants.ToList();
        }

        return question;
    }
}
