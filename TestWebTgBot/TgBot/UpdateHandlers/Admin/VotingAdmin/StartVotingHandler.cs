using TestWebTgBot.Models.Dbo;
using TestWebTgBot.Models.TgModels;
using TestWebTgBot.Services.Admin;
using TestWebTgBot.Utils.Buttons;

namespace TestWebTgBot.TgBot.UpdateHandlers.Admin.VotingAdmin;

public class StartVotingHandler : IUpdateHandler
{
    private readonly TelegramBot _telegramBot;
    private readonly IAdminService _adminService;

    public StartVotingHandler(TelegramBot telegramBot, IAdminService adminService)
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

        var buttonsThereIsNoVoting = AdminButtonsHelper.ButtonsNoVotingContext();
        
        if (lastVoting == null)
        {
            await _telegramBot.EditMessageText(
                chatId,
                messageId,
                "На данный момент голосования отсутствуют.",
                buttonsThereIsNoVoting
            );
            return;
        }

        lastVoting.IsStart = true;
        lastVoting = await _adminService.SetVotingStartAsync(lastVoting);
        
        var buttonContextOnlineVoting = AdminButtonsHelper.ButtonsAdminVotingContext(lastVoting.IsStart);
        
        var votingStatus = lastVoting.IsStart ? "активно" : "завершено";
        if (lastVoting.Result == 0 && lastVoting.IsStart == false)
        {
            votingStatus = "не активно";
        }
        var message = $"Последнее голосование:\n" +
                      $"Вопрос: {lastVoting.Question}\n" +
                      $"1. {lastVoting.OptionFirst}\n" +
                      $"2. {lastVoting.OptionSecond}\n" +
                      $"Статус: {votingStatus}";

        await _telegramBot.EditMessageText(chatId, messageId, message, buttonContextOnlineVoting);
    }

    public bool CanHandle(Update update)
    {
        if (update.CallbackQuery?.Message is not null && update.CallbackQuery?.From.Id is not null)
        {
            return update.CallbackQuery.Data == "StartVotingAdmin";
        }
        return false;
    }
}