using TestWebTgBot.Models.Dbo;
using TestWebTgBot.Models.Internal;
using TestWebTgBot.Models.TgModels;
using TestWebTgBot.Repositories.User;
using TestWebTgBot.Services.User;
using TestWebTgBot.TgBot;
using TestWebTgBot.TgBot.UpdateHandlers;
using TestWebTgBot.Utils;

namespace TestWebTgBot.Services.Telegram;

public class TelegramBotService : ITelegramBotService
{
    private readonly IEnumerable<IUpdateHandler> _updateHandlers;
    private readonly IUserService _userService;


    public TelegramBotService(IEnumerable<IUpdateHandler> updateHandlers, IUserService userService)
    {
        _updateHandlers = updateHandlers;
        _userService = userService;
    }
    public async Task OnUpdate(Update update)
    {
        var userData = new UserData();
        var userId = update.Message?.From?.Id ??
                     update.CallbackQuery?.From.Id;
        if (userId is not null)
        {
            var user = await _userService.CreateUserIfNotExistAsync(new UserEntity
            {
                Id = userId.Value,
                IsAdmin = false,
                IsSubscribed = false,
                UserFullName = "Unknown",
                UserChatState = UserChatStates.Default
            });
            userData.Id = user.Id;
            userData.IsAdmin = user.IsAdmin;
            userData.IsSubscribed = user.IsSubscribed;
            userData.UserChatState = user.UserChatState;
        }
        update.UserData = userData;
        
        foreach (var updateHandler in _updateHandlers)
        {
            if (updateHandler.CanHandle(update))
            {
                await updateHandler.HandleUpdateAsync(update);
            }
        }
        
    }
}