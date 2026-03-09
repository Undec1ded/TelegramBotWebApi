using TestWebTgBot.Models.Dbo;
using TestWebTgBot.Models.Internal;
using TestWebTgBot.Models.TgModels;
using TestWebTgBot.Services.Quiz;
using TestWebTgBot.Services.User;
using TestWebTgBot.Utils.Buttons;

namespace TestWebTgBot.TgBot.UpdateHandlers.Admin.QuizAdmin.ViewEdit.EditQuestion.Detailed;

public class AddQuizQuestionVariantMessageHandler : IUpdateHandler
{
    private readonly TelegramBot _telegramBot;
    private readonly IQuizService _quizService;
    private readonly IUserService _userService;

    public AddQuizQuestionVariantMessageHandler(
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
        var chatId = update.UserData.Id;
        var messageId = update.Message!.MessageId;
        var questionId = await _userService.GetEditingQuizQuestionIdForAdmin(chatId);

        if (questionId is null)
        {
            return;
        }

        var question = await _quizService.GetQuestionByIdAsync(questionId.Value);

        await _userService.SetStateDefault(new UserEntity
        {
            Id = chatId,
        });

        await _quizService.AddVariantToQuestion(new VariantEntity()
        {
            QuestionId = questionId.Value,
            Text = update.Message!.Text!,
            IsCorrect = question.Variants is null || question.Variants.Count == 0
        });

        var quiz = await _quizService.GetCurrentQuiz();

        var message = MakeMessageFromQuiz(quiz);

        var buttons = AdminButtonsHelper.CreateEditQuizQuestionsMenu(quiz);

        await _telegramBot.SendTextMessage(
            chatId,
            message,
            buttons);
    }

    public bool CanHandle(Update update)
    {
        if (update.Message?.From is not null &&
            update.Message?.Text is not null &&
            update.UserData.UserChatState == UserChatStates.WaitAdminVariantEntering)
        {
            return true;
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

        message += "\n\nВыберите вопрос для редактирования:";

        return message;
    }
}
