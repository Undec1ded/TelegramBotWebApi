using TestWebTgBot.Models.TgModels;
using TestWebTgBot.Services.Admin;
using TestWebTgBot.Utils.Buttons;

namespace TestWebTgBot.TgBot.UpdateHandlers.Admin.VotingAdmin;

public class ResultVotingAdminHandler : IUpdateHandler
{
    private readonly TelegramBot _telegramBot;
    private readonly IAdminService _adminService;

    public ResultVotingAdminHandler(TelegramBot telegramBot, IAdminService adminService)
    {
        _telegramBot = telegramBot;
        _adminService = adminService;
    }

    public async Task HandleUpdateAsync(Update update)
    {
        var chatId = update.CallbackQuery!.From.Id;
        var messageId = update.CallbackQuery!.Message!.MessageId;

        // Получаем последние 5 голосований
        var allVotingResults = await _adminService.GetAllVotingResultsAsync();
        var lastFiveVotings = allVotingResults.OrderByDescending(v => v.Id).Take(5).ToList();

        if (!lastFiveVotings.Any())
        {
            await _telegramBot.EditMessageText(
                chatId,
                messageId,
                "Нет доступных результатов голосования.",
                AdminButtonsHelper.ButtonGoToAdminStartPanel()
            );
            return;
        }

        string resultsMessage = "Результаты последних голосований:\n\n";

        foreach (var voting in lastFiveVotings)
        {
            resultsMessage += $"Вопрос: {voting.Question}\n" +
                              $"1. {voting.OptionFirst} - {voting.Result * 100:0.##}%\n" +
                              $"2. {voting.OptionSecond} - {100 - (voting.Result * 100):0.##}%\n\n";
        }

        // Отправляем сообщение с результатами и кнопкой "Вернуться в меню администратора"
        await _telegramBot.EditMessageText(
            chatId,
            messageId,
            resultsMessage,
            AdminButtonsHelper.ButtonGoToAdminStartPanel()
        );
    }

    public bool CanHandle(Update update)
    {
        if (update.CallbackQuery?.Message is not null && update.CallbackQuery?.From.Id is not null)
        {
            return update.CallbackQuery.Data == "ResultVotingAdmin";
        }
        return false;
    }
}
