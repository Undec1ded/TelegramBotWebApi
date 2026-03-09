using TestWebTgBot.Models.TgModels;
using TestWebTgBot.Services.Admin;
using TestWebTgBot.Services.Quiz;
using TestWebTgBot.Utils.Buttons;

namespace TestWebTgBot.TgBot.UpdateHandlers.Admin.QuizAdmin;

public class GoToAdminQuizMenuHandler : IUpdateHandler
{
    private readonly TelegramBot _telegramBot;
    private readonly IQuizService _quizService;

    public GoToAdminQuizMenuHandler(
        TelegramBot telegramBot,
        IQuizService quizService)
    {
        _telegramBot = telegramBot;
        _quizService = quizService;
    }

    public async Task HandleUpdateAsync(Update update)
    {
        var chatId = update.CallbackQuery!.From.Id;
        var messageId = update.CallbackQuery!.Message!.MessageId;

        var isQuizStart = await _quizService.IsLastQuizActiveAsync();
        
        var buttonsQuizMain = AdminButtonsHelper.CreateQuizMenu(isQuizStart);
        
        await _telegramBot.EditMessageText(chatId, messageId, "Меню управления Квизом",
            buttonsQuizMain);
    }

    public bool CanHandle(Update update)
    {
        if (update.CallbackQuery?.Message is not null && update.CallbackQuery?.From.Id is not null)
        {
            return update.CallbackQuery.Data == "AdminQuizMain";
        }
        return false;
    }
}
