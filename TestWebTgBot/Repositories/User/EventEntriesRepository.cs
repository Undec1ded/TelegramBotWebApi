using System.Data;
using Dapper;

namespace TestWebTgBot.Repositories.User;

public class EventEntriesRepository : IEventEntriesRepository
{
    private readonly IDbConnection _dbConnection;

    public EventEntriesRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task SetIsNotifiedAsync(int eventId)
    {
        const string sql = "UPDATE EventEntries SET IsNotified = 1 WHERE IdEvent = @IdEvent";
        await _dbConnection.ExecuteAsync(sql, new { IdEvent = eventId });
    }
}