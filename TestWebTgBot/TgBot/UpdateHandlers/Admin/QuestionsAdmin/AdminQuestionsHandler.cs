using TestWebTgBot.Models.TgModels;
using TestWebTgBot.Services.Admin;
using TestWebTgBot.Utils.Buttons;

namespace TestWebTgBot.TgBot.UpdateHandlers.Admin.QuestionsAdmin;

public class AdminQuestionsHandler: IUpdateHandler
{
    private readonly TelegramBot _telegramBot;
    private readonly IAdminService _adminService;

    public AdminQuestionsHandler(TelegramBot telegramBot, IAdminService adminService)
    {
        _telegramBot = telegramBot;
        _adminService = adminService;
    }

    public async Task HandleUpdateAsync(Update update)
    {
        var chatId = update.CallbackQuery!.From.Id;
        var messageId = update.CallbackQuery!.Message!.MessageId;

        var buttonGoToMainAdminPanel = AdminButtonsHelper.ButtonGoToAdminStartPanel();
        var buttonContext = AdminButtonsHelper.ButtonsAdminQuestionsContext();
        
        var questions = await _adminService.GetQuestionsAsync();
        if (!questions.Any())
        {
            await _telegramBot.EditMessageText(chatId, messageId,
                "На данный момент отсутствуют вопросы от пользователей.", buttonGoToMainAdminPanel);
            return;
        }
        
        var messageBuilder = new System.Text.StringBuilder("Список вопросов от пользователей:\n\n");
        foreach (var question in questions)
        {
            messageBuilder.AppendLine($"- {question.UserFullName}: {question.Question}");
        }
        
        await _telegramBot.EditMessageText(chatId, messageId,messageBuilder.ToString(), buttonContext);
    }

    public bool CanHandle(Update update)
    {
        if (update.CallbackQuery?.Message is not null && update.CallbackQuery?.From.Id is not null)
        {
            return update.CallbackQuery.Data == "AdminQuestionsToSpeaker";
        }

        return false;
    }
}