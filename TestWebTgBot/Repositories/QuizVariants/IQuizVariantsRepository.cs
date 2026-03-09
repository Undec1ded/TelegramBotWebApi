using TestWebTgBot.Models.Dbo;

namespace TestWebTgBot.Repositories.QuizVariants;

public interface IQuizVariantsRepository
{
    Task AddVariant(VariantEntity variant);
    Task RemoveVariantAsync(long variantId);
}
