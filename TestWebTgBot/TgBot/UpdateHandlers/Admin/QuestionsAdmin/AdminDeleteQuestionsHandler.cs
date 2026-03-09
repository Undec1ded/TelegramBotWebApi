using TestWebTgBot.Models.Dbo;
using TestWebTgBot.Models.TgModels;
using TestWebTgBot.Services.Admin;
using TestWebTgBot.Utils.Buttons;

namespace TestWebTgBot.TgBot.UpdateHandlers.Admin.QuestionsAdmin;

public class AdminDeleteQuestionsHandler : IUpdateHandler
{
    private readonly TelegramBot _telegramBot;
    private readonly IAdminService _adminService;

    public AdminDeleteQuestionsHandler(TelegramBot telegramBot, IAdminService adminService)
    {
        _telegramBot = telegramBot;
        _adminService = adminService;
    }

    public async Task HandleUpdateAsync(Update update)
    {
        var chatId = update.CallbackQuery!.From.Id;
        var messageId = update.CallbackQuery!.Message!.MessageId;

        var buttonGoToAdminPanel = AdminButtonsHelper.ButtonGoToAdminStartPanel();

        var questions = new QuestionEntity();
        questions = await _adminService.DeleteQuestionsAsync(questions);

        await _telegramBot.EditMessageText(chatId, messageId, "Список вопросов отчистен", buttonGoToAdminPanel);
    }

    public bool CanHandle(Update update)
    {
        if (update.CallbackQuery?.Message is not null && update.CallbackQuery?.From.Id is not null)
        {
            return update.CallbackQuery.Data == "DeleteQuestions";
        }
        return false;
    }
}