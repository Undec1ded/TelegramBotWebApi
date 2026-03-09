using TestWebTgBot.Models.Dbo;
using TestWebTgBot.Models.Internal;
using TestWebTgBot.Models.TgModels;
using TestWebTgBot.Services.User;
using TestWebTgBot.Utils.Buttons;

namespace TestWebTgBot.TgBot.UpdateHandlers.User.UserQuestions;

public class UserWriteQuestionHandler : IUpdateHandler
{
    private readonly TelegramBot _telegramBot;
    private readonly IUserService _userService;

    public UserWriteQuestionHandler(TelegramBot telegramBot, IUserService userService)
    {
        _telegramBot = telegramBot;
        _userService = userService;
    }

    public async Task HandleUpdateAsync(Update update)
    {
        var chatId = update.Message!.From!.Id;
        var question = update.Message.Text;

        var buttonsNewQuestion = ButtonsHelper.ButtonsNewQuestion();
        
        var user = new UserEntity();
        user.Id = chatId;

        var userQuestion = new QuestionEntity();
        userQuestion.UserId = chatId;
        userQuestion.Question = question;
        
        user = await _userService.SetStateDefault(user);
        userQuestion = await _userService.CreateQuestionAsync(userQuestion);

        await _telegramBot.SendTextMessage(chatId, "Вопрос сохранен.", buttonsNewQuestion);
    }

    public bool CanHandle(Update update)
    {
        if (update.Message?.Text is not null &&
            update.Message.From is not null &&
            update.UserData.UserChatState == UserChatStates.WaitUserQuestion)
        {
            return true;
        }
        return false;
    }
}