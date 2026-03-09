using TestWebTgBot.Models.Dbo;
using TestWebTgBot.Services.User;
using TestWebTgBot.Utils.Buttons;

namespace TestWebTgBot.TgBot.UpdateHandlers.User.UserVoting;

public class UserVoteHandler : IUpdateHandler
{
    private readonly TelegramBot _telegramBot;
    private readonly IUserService _userService;

    public UserVoteHandler(TelegramBot telegramBot, IUserService userService)
    {
        _telegramBot = telegramBot;
        _userService = userService;
    }

    public async Task HandleUpdateAsync(Models.TgModels.Update update)
    {
        var chatId = update.CallbackQuery!.From.Id;
        var messageId = update.CallbackQuery!.Message!.MessageId;
        var callbackData = update.CallbackQuery.Data!;

        var buttonGoVoting = ButtonsHelper.ButtonsGoToVoting();

        var option = callbackData == "UserVoted_0" ? false : true;

        var lastVoting = (await _userService.GetAllVotingResultsAsync())
            .OrderByDescending(v => v.Id)
            .FirstOrDefault();

        if (lastVoting == null || !lastVoting.IsStart)
        {
            await _telegramBot.EditMessageText(chatId, messageId, "Голосование завершено.", buttonGoVoting);
            return;
        }

        var vote = new VotingUsersEntity
        {
            VotingId = lastVoting.Id,
            UserId = chatId,
            Result = option
        };

        await _userService.RegisterUserVoteAsync(vote);

        var voteCount = await _userService.GetVoteCountByVotingIdAsync(lastVoting.Id);

        if (voteCount % 2 == 0)
        {
            var votingUsers = await _userService.GetVotingUsersByVotingIdAsync(lastVoting.Id);
            var votingResults = await _userService.GetVotingResultsAsync(lastVoting.Id);

            foreach (var votingUser in votingUsers)
            {
                var updatedText = $"Результаты голосования:\n\n" +
                                  $"Вопрос: {lastVoting.Question}\n" +
                                  $"1. {lastVoting.OptionFirst}: {votingResults.OptionFirstVotes} голосов\n" +
                                  $"2. {lastVoting.OptionSecond}: {votingResults.OptionSecondVotes} голосов";

                if (votingUser.VotingMessageId.HasValue && votingUser.VotingMessageId.Value != 0)
                {
                    await _telegramBot.EditMessageText(
                        votingUser.UserId,
                        votingUser.VotingMessageId.Value,
                        updatedText,
                        buttonGoVoting
                    );
                }
            }
        }

        await _telegramBot.EditMessageText(chatId, messageId, "Ваш голос учтен. Спасибо за участие!", buttonGoVoting);
    }

    public bool CanHandle(Models.TgModels.Update update)
    {
        return update.CallbackQuery?.Data?.StartsWith("UserVoted_") ?? false;
    }
}
