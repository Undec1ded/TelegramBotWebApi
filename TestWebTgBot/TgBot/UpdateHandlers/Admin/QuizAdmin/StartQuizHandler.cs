using TestWebTgBot.Models.Dbo;
using TestWebTgBot.Models.TgModels;
using TestWebTgBot.Services.Admin;
using TestWebTgBot.Services.User;
using TestWebTgBot.Utils.Buttons;

namespace TestWebTgBot.TgBot.UpdateHandlers.Admin.QuizAdmin;

public class StartQuizHandler : IUpdateHandler
{
    private readonly TelegramBot _telegramBot;
    private readonly IAdminService _adminService;

    public StartQuizHandler(TelegramBot telegramBot, IAdminService adminService)
    {
        _telegramBot = telegramBot;
        _adminService = adminService;
    }

    public async Task HandleUpdateAsync(Update update)
    {
        var chatId = update.CallbackQuery!.From.Id;
        var messageId = update.CallbackQuery!.Message!.MessageId;
        
        var isLastQuizActive = await _adminService.IsLastQuizActiveAsync();
        var goToMainQuizMenu = AdminButtonsHelper.ButtonsGoToQuizMenu();

        if (isLastQuizActive)
        {
            await _telegramBot.EditMessageText(chatId, messageId, "Сначала закончите предыдущий квиз",
                goToMainQuizMenu);
            return;
        }
        var quiz = new QuizAdminEntity()
        {
            IsStart = true
        };
        quiz = await _adminService.StartQuizAsync(quiz);
        
        var buttonsAdminMain = AdminButtonsHelper.CreateQuizMenu(quiz.IsStart);
        
        await _telegramBot.EditMessageText(chatId, messageId, "Меню управления Квизом", buttonsAdminMain);
    }

    public bool CanHandle(Update update)
    {
        if (update.CallbackQuery?.Message is not null && update.CallbackQuery?.From.Id is not null)
        {
            return update.CallbackQuery.Data == "StartQuiz";
        }
        return false;
    }
}