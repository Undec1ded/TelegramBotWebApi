using TestWebTgBot.Models.Dbo;

namespace TestWebTgBot.Repositories.User;

public interface IEventEntriesRepository
{
    Task SetIsNotifiedAsync(int eventId);
}