using TestWebTgBot.Models.Dbo;
using TestWebTgBot.Repositories.Quiz;
using TestWebTgBot.Repositories.QuizAnswer;
using TestWebTgBot.Repositories.QuizQuestion;
using TestWebTgBot.Repositories.QuizVariants;
using TestWebTgBot.TgBot;
using TestWebTgBot.Utils;
using TestWebTgBot.Utils.Buttons;

namespace TestWebTgBot.Services.Quiz;

public class QuizService : IQuizService
{
    private readonly TelegramBot _telegramBot;
    private readonly IQuizRepository _quizRepository;
    private readonly IQuizQuestionRepository _quizQuestionRepository;
    private readonly IQuizAnswerRepository _quizAnswerRepository;
    private readonly IQuizVariantsRepository _quizVariantsRepository;

    public QuizService(
        TelegramBot telegramBot,
        IQuizRepository quizRepository,
        IQuizQuestionRepository quizQuestionRepository,
        IQuizAnswerRepository quizAnswerRepository,
        IQuizVariantsRepository quizVariantsRepository)
    {
        _telegramBot = telegramBot;
        _quizRepository = quizRepository;
        _quizQuestionRepository = quizQuestionRepository;
        _quizAnswerRepository = quizAnswerRepository;
        _quizVariantsRepository = quizVariantsRepository;
    }

    public async Task AddUserAnswerAsync(long userId, long questionId, long answerId, int messageId)
    {
        var quiz = await _quizRepository.GetActiveQuizDetailedAsync();
        var questions = await _quizQuestionRepository.GetQuestionsByQuizAsync(quiz.Id);
        var question = questions.FirstOrDefault(q => q.Id == questionId);
        if (question is null)
        {
            // чето-странное квиз мб закончился
            return;
        }

        var userAnswer = new UserQuizAnswerEntity
        {
            UserId = userId,
            QuestionId = questionId,
            VariantId = answerId,
            MessageId = messageId
        };

        await _quizAnswerRepository.AddUserAnswerAsync(userAnswer);

        var nextQuestion = questions.FirstOrDefault(q => q.Id > questionId);
        if (nextQuestion is null)
        {
            // квиз закончился
            var userAnswers = await _quizAnswerRepository.GetUserQuizAnswers(userId, quiz.Id);
            var messagesToDeleteIds = userAnswers
                .Where(a => a.MessageId is not null)
                .Select(a => a.MessageId!.Value).ToList();
            await _telegramBot.DeleteMessages(userId, messagesToDeleteIds);

            await _quizAnswerRepository.ClearUserAnswersMessages(userId, quiz.Id);
            // отправляем сообщение о завершении квиза
            return;
        }

        await _telegramBot.SendTextMessage(
            userId,
            nextQuestion.Question,
            ButtonsHelper.CreateVariantsButtons(nextQuestion));
    }

    public async Task StartQuizForUser(long userId, int messageId)
    {
        var quiz = await _quizRepository.GetActiveQuizDetailedAsync();
        if (quiz is null)
        {
            //что-то пошло не так, квиз мб не активен
            return;
        }
        var question = await _quizQuestionRepository.GetFirstQuestionForQuizAsync(quiz.Id);

        if (question is null)
        {
            //что-то пошло не так, вопросы не найдены
            return;
        }

        await _telegramBot.EditMessageText(userId,
            messageId,
            question.Question,
            ButtonsHelper.CreateVariantsButtons(question));
    }

    public async Task ActivateQuizAsync(long? quizId)
    {
        if (quizId is not null)
        {
            var quiz = await _quizRepository.GetQuizByIdAsync(quizId.Value);

            if (quiz is null)
            {
                // квиз не найден
                return;
            }

            await _quizRepository.DeactivateAllQuizzesAsync(DateTime.Now.ToMoscowTime());
            await _quizRepository.ActivateQuizAsync(quizId.Value, DateTime.Now.ToMoscowTime());
        }

        var lastQuiz = await _quizRepository.GetLastQuizDetailedAsync();
        if (lastQuiz is null)
        {
            // нет квизов вообще
            return;
        }

        await _quizRepository.DeactivateAllQuizzesAsync(DateTime.Now.ToMoscowTime());
        await _quizRepository.ActivateQuizAsync(lastQuiz.Id, DateTime.Now.ToMoscowTime());
    }

    public Task DeactivateQuizAsync()
    {
        return _quizRepository.DeactivateAllQuizzesAsync(DateTime.Now.ToMoscowTime());
    }

    public async Task<bool> ActiveQuizExistAsync()
    {
        var activeQuiz = await _quizRepository.GetActiveQuizDetailedAsync();
        return activeQuiz is not null;
    }

    public async Task AddQuestionToQuiz(QuizQuestionEntity question)
    {
        var quiz = await _quizRepository.GetQuizByIdAsync(question.QuizId);

        if (quiz is null)
        {
            // квиз не найден
            return;
        }

        await _quizQuestionRepository.AddQuestionAsync(question);
    }

    public async Task AddVariantToQuestion(VariantEntity variant)
    {
        var question = await _quizQuestionRepository.GetQuestionByIdAsync(variant.QuestionId);

        if (question is null)
        {
            // вопрос не найден
            return;
        }

        await _quizVariantsRepository.AddVariant(variant);
    }

    public async Task RemoveVariantAsync(long variantId)
    {
        await _quizVariantsRepository.RemoveVariantAsync(variantId);
    }

    public async Task<QuizEntity> GetCurrentQuiz()
    {
        var currentActiveQuiz = await _quizRepository.GetLastQuizDetailedAsync();

        if (currentActiveQuiz is not null)
        {
            return currentActiveQuiz;
        }

        var newQuiz = await _quizRepository.CreateQuizAsync();
        return newQuiz;
    }

    public Task<bool> IsLastQuizActiveAsync()
    {
        return _quizRepository.IsLastQuizActiveAsync();
    }

    public async Task<QuizQuestionEntity> GetQuestionByIdAsync(long questionId)
    {
        var question = await _quizQuestionRepository.GetQuestionByIdAsync(questionId);
        if (question is null)
        {
            // вопрос не найден
            throw new Exception("Question not found");
        }

        return question;
    }

    public Task DeleteQuestionVariantAsync(long variantId)
    {
        return _quizVariantsRepository.RemoveVariantAsync(variantId);
    }
}
