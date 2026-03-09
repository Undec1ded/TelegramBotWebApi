using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using TestWebTgBot.Models.Dbo;
using TestWebTgBot.Models.Internal;
using TestWebTgBot.Models.TgModels;
using TestWebTgBot.Services.Admin;
using TestWebTgBot.Utils.Buttons;

namespace TestWebTgBot.TgBot.UpdateHandlers.Admin.VotingAdmin;

public class AddQuestionsForVotingHandler : IUpdateHandler
{
    private readonly TelegramBot _telegramBot;
    private readonly IAdminService _adminService;
    // Хранилище промежуточных состояний голосования
    private static readonly ConcurrentDictionary<long, VotingEntity> VotingInProgress = new();

    public AddQuestionsForVotingHandler(TelegramBot telegramBot, IAdminService adminService)
    {
        _telegramBot = telegramBot;
        _adminService = adminService;
    }

    public async Task HandleUpdateAsync(Update update)
    {
        var chatId = update.Message!.From!.Id;
        var messageText = update.Message.Text;

        var buttonsGoToAdminsStart = AdminButtonsHelper.ButtonGoToAdminStartPanel();
        var admin = new UserEntity { Id = chatId };

        var votingEntity = VotingInProgress.GetOrAdd(chatId, new VotingEntity());

        // Проверка на неподдерживаемые типы сообщений
        if (update.Message.Photo != null || update.Message.Sticker != null)
        {
            await _telegramBot.SendTextMessage(chatId, "Ошибка: изображения и стикеры не поддерживаются.");
            return;
        }

        if (update.Message.Voice != null)
        {
            await _telegramBot.SendTextMessage(chatId, "Ошибка: голосовые сообщения не поддерживаются.");
            return;
        }

        if (string.IsNullOrWhiteSpace(messageText))
        {
            await _telegramBot.SendTextMessage(chatId, "Ошибка: сообщение не должно быть пустым.");
            return;
        }

        // Проверка на наличие эмодзи
        if (ContainsEmoji(messageText))
        {
            await _telegramBot.SendTextMessage(chatId, "Ошибка: эмодзи запрещены в тексте. Попробуйте снова.");
            return;
        }

        switch (update.UserData.UserChatState)
        {
            case UserChatStates.WaitAdminAddVotingQuestion:
                votingEntity.Question = messageText;
                admin = await _adminService.SetStateWaitAdminAddFirstQuestionAsync(admin);

                await _telegramBot.SendTextMessage(
                    chatId,
                    "Введите первый вариант ответа:"
                );
                break;

            case UserChatStates.WaitAdminAddFirstQuestion:
                votingEntity.OptionFirst = messageText;
                admin = await _adminService.SetStateWaitAdminAddSecondQuestionAsync(admin);

                await _telegramBot.SendTextMessage(
                    chatId,
                    "Введите второй вариант ответа:"
                );
                break;

            case UserChatStates.WaitAdminAddSecondQuestion:
                votingEntity.OptionSecond = messageText;
                admin = await _adminService.SetStateDefaultAsync(admin);

                await _adminService.CreateVotingAsync(votingEntity);

                VotingInProgress.TryRemove(chatId, out _);

                await _telegramBot.SendTextMessage(
                    chatId,
                    "Голосование успешно создано!",
                    buttonsGoToAdminsStart
                );
                break;

            default:
                await _telegramBot.SendTextMessage(
                    chatId,
                    "Некорректное состояние пользователя. Попробуйте снова.",
                    buttonsGoToAdminsStart
                );
                break;
        }
    }

    private bool ContainsEmoji(string input)
    {
        // Регулярное выражение для поиска эмодзи
        var emojiPattern = @"[\uD83C-\uDBFF\uDC00-\uDFFF]|[\u2600-\u27BF]";
        return Regex.IsMatch(input, emojiPattern);
    }

    public bool CanHandle(Update update)
    {
        if (update.Message is not null && update.Message.From is not null)
        {
            var state = update.UserData.UserChatState;
            return state == UserChatStates.WaitAdminAddVotingQuestion ||
                   state == UserChatStates.WaitAdminAddFirstQuestion ||
                   state == UserChatStates.WaitAdminAddSecondQuestion;
        }
        return false;
    }
}
