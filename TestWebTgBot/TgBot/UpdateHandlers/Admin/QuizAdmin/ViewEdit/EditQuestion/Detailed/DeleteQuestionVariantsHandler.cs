using TestWebTgBot.Models.Dbo;
using TestWebTgBot.Models.TgModels;
using TestWebTgBot.Services.Quiz;
using TestWebTgBot.Utils.Buttons;

namespace TestWebTgBot.TgBot.UpdateHandlers.Admin.QuizAdmin.ViewEdit.EditQuestion.Detailed;

public class DeleteQuestionVariantsHandler : IUpdateHandler
{
    private readonly TelegramBot _telegramBot;
    private readonly IQuizService _quizService;

    public DeleteQuestionVariantsHandler(
        TelegramBot telegramBot,
        IQuizService quizService)
    {
        _telegramBot = telegramBot;
        _quizService = quizService;
    }

    public async Task HandleUpdateAsync(Update update)
    {
        var chatId = update.UserData.Id;
        var messageId = update.CallbackQuery!.Message!.MessageId;
        var questionId = long.Parse(update.CallbackQuery.Data.Split("_")[1]);

        var question = await _quizService.GetQuestionByIdAsync(questionId);
        var buttons = AdminButtonsHelper.CreateDeleteVariantsButtons(question);
        var message = MakeMessageFromQuestion(question);

        await _telegramBot.EditMessageText(
            chatId,
            messageId,
            message,
            buttons);
    }

    public bool CanHandle(Update update)
    {
        if (update.CallbackQuery?.Message is not null &&
            update.CallbackQuery?.From.Id is not null &&
            update.CallbackQuery.Data is not null)
        {
            return update.CallbackQuery.Data.StartsWith("DeleteQuestionVariants_");
        }
        return false;
    }

    private string MakeMessageFromQuestion(QuizQuestionEntity question)
    {
        var message = $"Вопрос: {question.Question}";

        message+= "\nКакой вариант удалить? Первый - правильный:";

        return message;
    }
}
