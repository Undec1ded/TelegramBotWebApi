using TestWebTgBot.Models.Dbo;
using TestWebTgBot.Models.TgModels;
using TestWebTgBot.Services.Quiz;
using TestWebTgBot.Services.User;

namespace TestWebTgBot.TgBot.UpdateHandlers.Admin.QuizAdmin.ViewEdit.EditQuestion.Detailed;

public class AddQuizQuestionVariantHandler : IUpdateHandler
{
    private readonly TelegramBot _telegramBot;
    private readonly IQuizService _quizService;
    private readonly IUserService _userService;

    public AddQuizQuestionVariantHandler(
        TelegramBot telegramBot,
        IQuizService quizService,
        IUserService userService)
    {
        _telegramBot = telegramBot;
        _quizService = quizService;
        _userService = userService;
    }

    public async Task HandleUpdateAsync(Update update)
    {
        var chatId = update.CallbackQuery!.From.Id;
        var messageId = update.CallbackQuery!.Message!.MessageId;
        var questionId = int.Parse(update.CallbackQuery.Data.Split("_")[1]);

        var question = await _quizService.GetQuestionByIdAsync(questionId);

        await _userService.SetStateAdminVariantEntering(new UserEntity
        {
            Id = chatId,
        }, questionId);

        await _telegramBot.EditMessageText(chatId, messageId, "Введите вариант ответа на вопрос: " +
            question.Question);
    }

    public bool CanHandle(Update update)
    {
        if (update.CallbackQuery?.Message is not null &&
            update.CallbackQuery?.From.Id is not null &&
            update.CallbackQuery.Data is not null)
        {
            return update.CallbackQuery.Data.StartsWith("AddQuestionVariant_");
        }
        return false;
    }
}
