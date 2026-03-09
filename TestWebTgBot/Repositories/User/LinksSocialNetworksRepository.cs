using System.Data;
using Dapper;
using TestWebTgBot.Models.Dbo;

namespace TestWebTgBot.Repositories.User;

public class LinksSocialNetworksRepository : ILinksSocialNetworksRepository
{
    private readonly IDbConnection _dbConnection;

    public LinksSocialNetworksRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }
    public async Task<LinksSocialNetworksEntity?> GetLinkEntityByNameAsync(string linkName)
    {
        var sql = "SELECT Id, Link, LinkName FROM LinksSocialNetworks WHERE LinkName = @LinkName";
        var entity = await _dbConnection.QuerySingleOrDefaultAsync<LinksSocialNetworksEntity>(sql, new { LinkName = linkName });
        return entity;
    }

}