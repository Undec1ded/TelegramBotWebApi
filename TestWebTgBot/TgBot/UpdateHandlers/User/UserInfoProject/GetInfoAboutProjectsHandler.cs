using TestWebTgBot.Models.TgModels;
using TestWebTgBot.Utils.Buttons;

namespace TestWebTgBot.TgBot.UpdateHandlers.User.UserInfoProject;

public class GetInfoAboutProjectsHandler : IUpdateHandler
{
    private readonly TelegramBot _telegramBot;

    public GetInfoAboutProjectsHandler(TelegramBot telegramBot)
    {
        _telegramBot = telegramBot;
    }

    public async Task HandleUpdateAsync(Update update)
    {
        var chatId = update.CallbackQuery!.From.Id;
        var messageId = update.CallbackQuery!.Message!.MessageId;

        var buttonGoToStartMenu = ButtonsHelper.ButtonGoToStartUserButtons();

        await _telegramBot.SendPhoto(chatId,
            "https://i.pinimg.com/736x/63/ba/0b/63ba0b96b67d74eff387a5b755f0c39f.jpg", "InfoProject");
        await _telegramBot.SendTextMessage(chatId, "Главное меню", buttonGoToStartMenu);
    }

    public bool CanHandle(Update update)
    {
        if (update.CallbackQuery?.Message is not null && update.CallbackQuery?.From.Id is not null)
        {
            return update.CallbackQuery.Data == "InformationAboutProjects";
        }
        return false;
    }
}