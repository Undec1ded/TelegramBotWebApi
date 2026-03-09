using TestWebTgBot.Models.Dbo;
using TestWebTgBot.Models.Internal;
using TestWebTgBot.Models.TgModels;
using TestWebTgBot.Services.Quiz;
using TestWebTgBot.Services.User;
using TestWebTgBot.Utils.Buttons;

namespace TestWebTgBot.TgBot.UpdateHandlers.Admin.QuizAdmin.ViewEdit.AddQuestion;

public class AddQuestionQuizMessageHandler : IUpdateHandler
{
    private readonly TelegramBot _telegramBot;
    private readonly IQuizService _quizService;
    private readonly IUserService _userService;

    public AddQuestionQuizMessageHandler(
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
        var message = update.Message;
        var messageId = message.MessageId;

        if (message is null)
        {
            return;
        }

        var questionText = message.Text ?? message.Caption;

        if (string.IsNullOrEmpty(questionText))
        {
            return;
        }

        string? imagesIds = null;
        if (update.Message?.Photo is not null &&
            update.Message.Photo.Any())
        {
            imagesIds = string.Join(",", update.Message.Photo.Select(p => p.FileId));
        }

        var currentQuiz = await _quizService.GetCurrentQuiz();

        var question = new QuizQuestionEntity
        {
            Question = questionText,
            QuizId = currentQuiz.Id,
            ImagesIds = imagesIds
        };

        await _quizService.AddQuestionToQuiz(question);
        await _userService.SetStateDefault(new UserEntity() { Id = chatId });

        if (currentQuiz.Questions is null)
        {
            currentQuiz.Questions = new List<QuizQuestionEntity>();
        }

        currentQuiz.Questions.Add(question);

        var messageText = MakeMessageFromQuiz(currentQuiz);

        var buttons = AdminButtonsHelper.CreateEditQuizMenu();

        await _telegramBot.SendTextMessage(
            chatId,
            messageText,
            buttons);
    }

    public bool CanHandle(Update update)
    {
        if (update.Message?.From is not null &&
                 update.UserData.UserChatState == UserChatStates.WaitAdminQuestionEntering)
        {
            return !string.IsNullOrEmpty(update.Message?.Text) ||
                   (update.Message?.Photo is not null &&
                    update.Message.Photo.Any() &&
                    !string.IsNullOrEmpty(update.Message?.Caption));
        }

        return false;
    }

    private string MakeMessageFromQuiz(QuizEntity quizEntity)
    {
        var message = "✅ Вопрос добавлен\nТекущий квиз:";

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
