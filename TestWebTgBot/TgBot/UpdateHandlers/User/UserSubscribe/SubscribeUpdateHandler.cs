using TestWebTgBot.Models.Dbo;
using TestWebTgBot.Models.TgModels;
using TestWebTgBot.Services.User;
using TestWebTgBot.Utils.Buttons;

namespace TestWebTgBot.TgBot.UpdateHandlers.User.UserSubscribe;

public class SubscribeUpdateHandler : IUpdateHandler
{
    private readonly TelegramBot _telegramBot;
    private readonly IUserService _userService;

    public SubscribeUpdateHandler(TelegramBot telegramBot, IUserService userService)
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
        user = await _userService.SetIsSubscribedOnAsync(user);
        user = await _userService.GetUserByTelegramId(chatId);
        bool isSubscribed = user!.IsSubscribed;

        
        var startPanelButtons = ButtonsHelper.ButtonsStartPanel(isSubscribed);
        
        await _telegramBot.EditMessageReplyMarkup(chatId, messageId, startPanelButtons);
    }

    public bool CanHandle(Update update)
    {
        if (update.CallbackQuery?.Message is not null && update.CallbackQuery?.From.Id is not null)
        {
            return update.CallbackQuery.Data == "NewsletterOn";
        }

        return false;
    }
}