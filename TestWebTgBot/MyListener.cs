using TestWebTgBot.Models.TgModels;
using TestWebTgBot.TgBot;

namespace TestWebTgBot;

public class MyListener : IUpdateListener
{
    private TelegramBot _telegramBot;

    public async Task OnUpdate(Update update, TelegramBot telegramBot)
    {
        if (update.Message?.Text is not null && update.Message.From is not null)
        {
            long chatId = update.Message.From.Id;
            string receivedText = update.Message.Text;

            await telegramBot.SendTextMessage(chatId, $"Echo: {receivedText}");
        }

        if (update.CallbackQuery?.Message is not null && update.CallbackQuery?.From is not null)
        {
            
        }
    }
}
