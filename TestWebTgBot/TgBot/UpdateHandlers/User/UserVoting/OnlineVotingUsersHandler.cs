using TestWebTgBot.Models.Dbo;
using TestWebTgBot.Models.TgModels;
using TestWebTgBot.Services.User;
using TestWebTgBot.Utils.Buttons;

namespace TestWebTgBot.TgBot.UpdateHandlers.User.UserVoting;

public class OnlineVotingUsersHandler : IUpdateHandler
{
    private readonly TelegramBot _telegramBot;
    private readonly IUserService _userService;

    public OnlineVotingUsersHandler(TelegramBot telegramBot, IUserService userService)
    {
        _telegramBot = telegramBot;
        _userService = userService;
    }

    public async Task HandleUpdateAsync(Update update)
    {
        var chatId = update.CallbackQuery!.From.Id;
        var messageId = update.CallbackQuery!.Message!.MessageId;

        var allVotingResults = await _userService.GetAllVotingResultsAsync();
        var lastVoting = allVotingResults.OrderByDescending(v => v.Id).FirstOrDefault();
        
        var buttonsVotingNotStartContext = ButtonsHelper.ButtonsVotingNotStartContext();
        var buttonGoToStartMenu = ButtonsHelper.ButtonGoToStartUserAfterVotingButtons();
        
        
        if (lastVoting == null || !lastVoting.IsStart)
        {
            await _telegramBot.EditMessageText(chatId, messageId, "Нет активных голосований.", buttonsVotingNotStartContext);
            return;
        }
        
        var hasVoted = await _userService.HasUserVotedAsync(chatId, lastVoting.Id);
        
        if (hasVoted)
        {
            var votingResults = await _userService.GetVotingResultsAsync(lastVoting.Id);
            
            var votingUser = new VotingUsersEntity();
            votingUser.UserId = chatId;
            votingUser.VotingId = lastVoting.Id;
            votingUser.VotingMessageId = messageId;
            votingUser = await _userService.AddVotingMessageIdAsync(votingUser);
            
            var resultsMessage = $"Результаты голосования:\n\n" +
                                 $"Вопрос: {lastVoting.Question}\n" +
                                 $"1. {lastVoting.OptionFirst}: {votingResults.OptionFirstVotes} голосов\n" +
                                 $"2. {lastVoting.OptionSecond}: {votingResults.OptionSecondVotes} голосов";

            await _telegramBot.EditMessageText(chatId, messageId, resultsMessage, buttonGoToStartMenu);
            return;
        }
        
        var buttonsVoting = ButtonsHelper.ButtonsVotingUsersOptions(
            lastVoting.OptionFirst!, lastVoting.OptionSecond!);
        
        await _telegramBot.EditMessageText(
            chatId,
            messageId,
            $"Голосование:\n\n{lastVoting.Question}",
            buttonsVoting
        );
    }

    public bool CanHandle(Update update)
    {
        return update.CallbackQuery?.Data == "OnlineVoting";
    }
}
