using TestWebTgBot.Models.Dbo;
using TestWebTgBot.Models.TgModels;
using TestWebTgBot.Services.User;
using TestWebTgBot.Utils.Buttons;

namespace TestWebTgBot.TgBot.UpdateHandlers.User.UserSocialLinks;

public class UserSocialLinksHandler: IUpdateHandler
{
    private readonly TelegramBot _telegramBot;
    private readonly IUserService _userService;

    public UserSocialLinksHandler(TelegramBot telegramBot , IUserService userService)
    {
        _telegramBot = telegramBot;
        _userService = userService;
    }

    public async Task HandleUpdateAsync(Update update)
    {
        var chatId = update.CallbackQuery!.From.Id;
        var messageId = update.CallbackQuery!.Message!.MessageId;

        var link = new LinksSocialNetworksEntity();

        link = await _userService.GetLinkEntityByNameAsync("h2CHT");
        
        var buttonsLinks = ButtonsHelper.ButtonsLinks($"\u27a4{link!.LinkName}", link.Link!);
        await _telegramBot.EditMessageText(chatId, messageId,"Ссылки на социальные сети:", buttonsLinks);
    }

    public bool CanHandle(Update update)
    {
        if (update.CallbackQuery?.Message is not null && update.CallbackQuery?.From.Id is not null)
        {
            return update.CallbackQuery.Data == "LinksToSocialNetworks";
        }
        return false;
    }
}