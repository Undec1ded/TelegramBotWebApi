using TestWebTgBot.Models.Dbo;
using TestWebTgBot.Models.TgModels;
using TestWebTgBot.Services.User;
using TestWebTgBot.Utils.Buttons;

namespace TestWebTgBot.TgBot.UpdateHandlers.User.UserVoting;

public class GoToStartButtonsAfterVotingHandler : IUpdateHandler
{
    private readonly TelegramBot _telegramBot;
    private readonly IUserService _userService;

    public GoToStartButtonsAfterVotingHandler(TelegramBot telegramBot, IUserService userService)
    {
        _telegramBot = telegramBot;
        _userService = userService;
    }

    public async Task HandleUpdateAsync(Update update)
    {
        var chatId = update.CallbackQuery!.From.Id;
        var messageId = update.CallbackQuery!.Message!.MessageId;
        
        var user = new UserEntity();
        user.Id = chatId;
        user = await _userService.SetStateDefault(user);
        
        var startPanelButtons = ButtonsHelper.ButtonsStartPanel(user.IsSubscribed);

        var allVotingResults = await _userService.GetAllVotingResultsAsync();
        var lastVoting = allVotingResults.OrderByDescending(v => v.Id).FirstOrDefault();
        
        if (lastVoting == null || !lastVoting.IsStart)
        {
            await _telegramBot.EditMessageText(chatId, messageId, "QQ", startPanelButtons);
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
            votingUser = await _userService.DeleteVotingMessageIdAsync(votingUser);
            await _telegramBot.EditMessageText(chatId, messageId, "QQ", startPanelButtons);
            return;
        }
        await _telegramBot.EditMessageText(chatId, messageId,"QQ", startPanelButtons);
    }

    public bool CanHandle(Update update)
    {
        if (update.CallbackQuery?.Message is not null && update.CallbackQuery?.From.Id is not null)
        {
            return update.CallbackQuery.Data == "StartMenuAfterVoting";
        }
        return false;
    }
}