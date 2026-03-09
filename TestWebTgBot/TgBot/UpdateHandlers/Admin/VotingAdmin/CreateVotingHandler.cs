using TestWebTgBot.Models.Dbo;
using TestWebTgBot.Models.TgModels;
using TestWebTgBot.Services.Admin;
using TestWebTgBot.Utils.Buttons;

namespace TestWebTgBot.TgBot.UpdateHandlers.Admin.VotingAdmin;

public class CreateVotingHandler : IUpdateHandler
{
    private readonly TelegramBot _telegramBot;
    private readonly IAdminService _adminService;

    public CreateVotingHandler(TelegramBot telegramBot, IAdminService adminService)
    {
        _telegramBot = telegramBot;
        _adminService = adminService;
    }

    public async Task HandleUpdateAsync(Update update)
    {
        var chatId = update.CallbackQuery!.From.Id;
        var messageId = update.CallbackQuery!.Message!.MessageId;
        
        var allVoting = await _adminService.GetAllVotingResultsAsync();
        var lastVoting = allVoting.OrderByDescending(v => v.Id).FirstOrDefault();

        var buttonsVotingIsStart = AdminButtonsHelper.ButtonsGoToMainVoting();
        
        if (lastVoting is not null && lastVoting.IsStart == true)
        {
            await _telegramBot.EditMessageText(
                chatId,
                messageId,
                "Для добавления нового голосования, закончите предыдущее",
                buttonsVotingIsStart
            );
            return;
        }
        var user = new UserEntity();
        user.Id = chatId;
        user = await _adminService.SetStateWaitAdminAddVotingQuestionAsync(user);
        
        var buttonGoToAdminMenu = AdminButtonsHelper.ButtonGoToAdminStartPanel();

        await _telegramBot.EditMessageText(chatId, messageId, "Введите вопрос:", buttonGoToAdminMenu);
    }

    public bool CanHandle(Update update)
    {
        if (update.CallbackQuery?.Message is not null && update.CallbackQuery?.From.Id is not null)
        {
            return update.CallbackQuery.Data == "CreateVotingAdmin";
        }
        return false;
    }
}