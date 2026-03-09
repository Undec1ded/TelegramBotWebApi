using System.Data;
using Dapper;
using TestWebTgBot.Models.Dbo;

namespace TestWebTgBot.Repositories.QuizVariants;

public class QuizVariantsRepository : IQuizVariantsRepository
{
    private readonly IDbConnection _connection;

    public QuizVariantsRepository(IDbConnection connection)
    {
        _connection = connection;
    }
    public Task AddVariant(VariantEntity variant)
    {
        var sql = @"
            INSERT INTO QuizVariant (Text, IsCorrect, QuestionId)
            VALUES (@Text, @IsCorrect, @QuestionId)";

        return _connection.ExecuteAsync(sql, variant);
    }

    public Task RemoveVariantAsync(long variantId)
    {
        var sql = @"
            DELETE FROM QuizVariant
            WHERE Id = @variantId";

        return _connection.ExecuteAsync(sql, new { variantId });
    }
}
