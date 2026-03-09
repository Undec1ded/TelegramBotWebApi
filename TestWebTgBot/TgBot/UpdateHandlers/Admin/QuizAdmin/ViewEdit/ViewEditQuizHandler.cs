using TestWebTgBot.Models.Dbo;
using TestWebTgBot.Models.TgModels;
using TestWebTgBot.Services.Quiz;
using TestWebTgBot.Utils.Buttons;

namespace TestWebTgBot.TgBot.UpdateHandlers.Admin.QuizAdmin.ViewEdit;

public class ViewEditQuizHandler : IUpdateHandler
{
    private readonly IQuizService _quizService;
    private readonly TelegramBot _telegramBot;

    public ViewEditQuizHandler(
        IQuizService quizService,
        TelegramBot telegramBot)
    {
        _quizService = quizService;
        _telegramBot = telegramBot;
    }

    public async Task HandleUpdateAsync(Update update)
    {
        var chatId = update.UserData.Id;
        var messageId = update.CallbackQuery!.Message!.MessageId;

        var quiz = await _quizService.GetCurrentQuiz();

        var buttons = AdminButtonsHelper.CreateEditQuizMenu();

        var message = MakeMessageFromQuiz(quiz);

        await _telegramBot.EditMessageText(
            chatId,
            messageId,
            message,
            buttons);
    }

    public bool CanHandle(Update update)
    {
        if (update.CallbackQuery?.Message is not null && update.CallbackQuery?.From.Id is not null)
        {
            return update.CallbackQuery.Data == "ViewEditQuiz";
        }
        return false;
    }

    private string MakeMessageFromQuiz(QuizEntity quizEntity)
    {
        var message = "Текущий квиз:";

        if (quizEntity.Questions is not null)
        {
            foreach (var question in quizEntity.Questions)
            {
                message += $"\n{question.Question}";
                if (question.Variants is not null)
                {
                    foreach (var variant in question.Variants.OrderBy(x=> x.Id))
                    {
                        message += "\n";
                        if (variant.IsCorrect)
                        {
                            message += "✅";
                        }
                        message += $"{variant.Text}";
                    }
                }
            }
        }

        return message;
    }
}
