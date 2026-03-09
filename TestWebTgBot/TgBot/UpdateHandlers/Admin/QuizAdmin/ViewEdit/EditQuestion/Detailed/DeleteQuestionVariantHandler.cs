using TestWebTgBot.Models.Dbo;
using TestWebTgBot.Models.TgModels;
using TestWebTgBot.Services.Quiz;
using TestWebTgBot.Utils.Buttons;

namespace TestWebTgBot.TgBot.UpdateHandlers.Admin.QuizAdmin.ViewEdit.EditQuestion.Detailed;

public class DeleteQuestionVariantHandler : IUpdateHandler
{
    private readonly TelegramBot _telegramBot;
    private readonly IQuizService _quizService;

    public DeleteQuestionVariantHandler(
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
        var variantId = long.Parse(update.CallbackQuery.Data.Split("_")[1]);
        var questionId = long.Parse(update.CallbackQuery.Data.Split("_")[2]);

        await _quizService.DeleteQuestionVariantAsync(variantId);

        var question = await _quizService.GetQuestionByIdAsync(questionId);

        var buttons = AdminButtonsHelper.CreateEditQuestionMenu(question);
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
            return update.CallbackQuery.Data.StartsWith("Dqv_");
        }
        return false;
    }

    private string MakeMessageFromQuestion(QuizQuestionEntity question)
    {
        var message = $"✅ Вариант удален\nВопрос: {question.Question}";

        if (question.Variants is not null)
        {
            message += "\n\nВарианты ответов:";
            foreach (var variant in question.Variants.OrderBy(x=> x.Id))
            {
                message += $"\n{variant.Text}";
            }
        }

        message += "\nПравильный ответ - первый вариант";

        return message;
    }
}
