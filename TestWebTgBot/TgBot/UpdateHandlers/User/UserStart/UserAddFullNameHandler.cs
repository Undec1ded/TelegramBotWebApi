using System.Text.RegularExpressions;
using TestWebTgBot.Models.Dbo;
using TestWebTgBot.Models.Internal;
using TestWebTgBot.Models.TgModels;
using TestWebTgBot.Services.User;
using TestWebTgBot.Utils.Buttons;

namespace TestWebTgBot.TgBot.UpdateHandlers.User.UserStart;

public class UserAddFullNameHandler : IUpdateHandler
{
    private readonly TelegramBot _telegramBot;
    private readonly IUserService _userService;

    public UserAddFullNameHandler(TelegramBot telegramBot, IUserService userService)
    {
        _telegramBot = telegramBot;
        _userService = userService;
    }

    public async Task HandleUpdateAsync(Update update)
    {
        if (update.Message?.Text is null)
        {
            await _telegramBot.SendTextMessage(update.Message!.From!.Id, "Некорректный ввод ФИО, попробуйте снова.");
            return;
        }

        var chatId = update.Message!.From!.Id;
        var userFullName = update.Message.Text?.Trim();

        if (!string.IsNullOrEmpty(userFullName) && userFullName[0] == '/')
        {
            await _telegramBot.SendTextMessage(chatId, "Пожалуйста, введите своё ФИО, а не команду.");
            return;
        }

        if (ContainsEmoji(userFullName!))
        {
            await _telegramBot.SendTextMessage(chatId, "Ошибка: ФИО не должно содержать эмодзи.");
            return;
        }
        // Проверка на минимальное количество слов
        if (userFullName.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length < 2)
        {
            await _telegramBot.SendTextMessage(chatId, "Ошибка: ФИО должно состоять минимум из двух слов.");
            return;
        }

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

        var buttonRegistration = ButtonsHelper.ButtonsRegistration();
        var user = new UserEntity
        {
            Id = chatId,
            UserFullName = userFullName
        };

        user = await _userService.SetStateDefault(user);
        user = await _userService.SetUserFullName(user);

        await _telegramBot.SendTextMessage(chatId, $"Добро пожаловать, {userFullName}!", buttonRegistration);
    }

    private bool ContainsEmoji(string text)
    {
        var emojiRegex = new Regex(@"\p{Cs}");
        return emojiRegex.IsMatch(text);
    }

    public bool CanHandle(Update update)
    {
        if (update.Message is not null &&
            update.UserData.UserChatState == UserChatStates.WaitUserFullName)
        {
            return true;
        }
        return false;
    }
}
