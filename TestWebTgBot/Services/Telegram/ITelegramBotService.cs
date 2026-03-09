using TestWebTgBot.Models.TgModels;

namespace TestWebTgBot.Services.Telegram;

public interface ITelegramBotService
{
    Task OnUpdate(Update update);
}