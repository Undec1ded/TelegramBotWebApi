using TestWebTgBot.Models.Dbo;

namespace TestWebTgBot.Repositories.User;

public interface ILinksSocialNetworksRepository
{
    Task<LinksSocialNetworksEntity?> GetLinkEntityByNameAsync(string linkName);
}