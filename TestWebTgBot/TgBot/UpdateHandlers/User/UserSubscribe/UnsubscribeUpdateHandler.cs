using TestWebTgBot.Models.Dbo;
using TestWebTgBot.Models.TgModels;
using TestWebTgBot.Services.User;
using TestWebTgBot.Utils.Buttons;

namespace TestWebTgBot.TgBot.UpdateHandlers.User.UserSubscribe;

public class UnsubscribeUpdateHandler : IUpdateHandler
{
    private readonly TelegramBot _telegramBot;
    private readonly IUserService _userService;

    public UnsubscribeUpdateHandler(TelegramBot telegramBot, IUserService userService)
    {
        _telegramBot = telegramBot;
        _userService = userService;
    }
    public async Task HandleUpdateAsync(Update update)
    {
        
        long chatId = update.CallbackQuery!.From.Id;
        int messageId = update.CallbackQuery!.Message!.MessageId;    
        
        var user = new UserEntity();
        user.Id = chatId;
        user = await _userService.SetIsSubscribedOffAsync(user);
        bool isSubscribed = user.IsSubscribed;
        var startPanelButtons = ButtonsHelper.ButtonsStartPanel(isSubscribed);

        await _telegramBot.EditMessageReplyMarkup(chatId, messageId, startPanelButtons);
    }

    bool IUpdateHandler.CanHandle(Update update)
    {
        return CanHandle(update);
    }

    public bool CanHandle(Update update)
    {
        if (update.CallbackQuery?.Message is not null && update.CallbackQuery?.From.Id is not null)
        {
            return update.CallbackQuery.Data == "NewsletterOff";
        }
        return false;
    }
}