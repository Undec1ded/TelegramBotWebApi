using TestWebTgBot.Models.TgModels;

namespace TestWebTgBot.TgBot.UpdateHandlers;

public interface IUpdateHandler
{
    Task HandleUpdateAsync(Update update);
    bool CanHandle(Update update);
}