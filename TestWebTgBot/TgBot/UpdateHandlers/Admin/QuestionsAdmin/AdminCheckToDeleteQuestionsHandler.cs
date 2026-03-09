using TestWebTgBot.Models.TgModels;
using TestWebTgBot.Services.Admin;
using TestWebTgBot.Utils.Buttons;

namespace TestWebTgBot.TgBot.UpdateHandlers.Admin.QuestionsAdmin;

public class AdminCheckToDeleteQuestionsHandler : IUpdateHandler
{
    private readonly TelegramBot _telegramBot;
    private readonly IAdminService _adminService;

    public AdminCheckToDeleteQuestionsHandler(TelegramBot telegramBot, IAdminService adminService)
    {
        _telegramBot = telegramBot;
        _adminService = adminService;
    }


    public async Task HandleUpdateAsync(Update update)
    {
        var chatId = update.CallbackQuery!.From.Id;
        var messageId = update.CallbackQuery!.Message!.MessageId;

        var buttonCheck = AdminButtonsHelper.ButtonsAdminQuestionsDeleteCheck();

        await _telegramBot.EditMessageText(chatId, messageId, "Поддвердите действие *отчистить список вопросов*", buttonCheck);
    }

    public bool CanHandle(Update update)
    {
        if (update.CallbackQuery?.Message is not null && update.CallbackQuery?.From.Id is not null)
        {
            return update.CallbackQuery.Data == "DeleteQuestionsCheck";
        }
        return false;
    }
}