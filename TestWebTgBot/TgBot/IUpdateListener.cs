using TestWebTgBot.Models.TgModels;

namespace TestWebTgBot.TgBot;

public interface IUpdateListener
{
    Task OnUpdate(Update update, TelegramBot telegramBot); 
}