using TestWebTgBot.Models.Dbo;
using TestWebTgBot.Models.Internal;
using TestWebTgBot.Models.TgModels;
using TestWebTgBot.Services.Admin;
using TestWebTgBot.Services.User;
using TestWebTgBot.Utils.Buttons;

namespace TestWebTgBot.TgBot.UpdateHandlers.Admin.News;

public class CreateNewsHandler : IUpdateHandler
    {
        private readonly TelegramBot _telegramBot;
        private readonly IAdminService _adminService;
        private readonly IUserService _userService;

        public CreateNewsHandler(TelegramBot telegramBot, IAdminService adminService, IUserService userService)
        {
            _telegramBot = telegramBot;
            _adminService = adminService;
            _userService = userService;
        }

        public async Task HandleUpdateAsync(Update update)
        {
            var chatId = update.Message!.From!.Id;

            var adminUser = new UserEntity { Id = chatId };
            await _adminService.SetStateDefaultAsync(adminUser);

            var buttonMainAdmin = AdminButtonsHelper.CrateAdminPanel();

            var photo = update.Message.Photo;
            var caption = update.Message.Caption ?? update.Message.Text;

            var subscribedUsers = await _userService.GetSubscribedUsersAsync();

            if (subscribedUsers == null || !subscribedUsers.Any())
            {
                await _telegramBot.SendTextMessage(chatId, "Нет подписанных пользователей для рассылки.", buttonMainAdmin);
                return;
            }

            if (photo != null && photo.Any())
            {
                var fileId = photo.Last().FileId;
                foreach (var user in subscribedUsers)
                {
                    await _telegramBot.SendPhoto(user.Id, fileId, caption);
                }

                await _telegramBot.SendTextMessage(chatId, "Новость с картинкой успешно отправлена всем подписчикам.", buttonMainAdmin);
            }
            else if (!string.IsNullOrWhiteSpace(caption))
            {
                // Если только текст
                foreach (var user in subscribedUsers)
                {
                    await _telegramBot.SendTextMessage(user.Id, caption);
                }

                await _telegramBot.SendTextMessage(chatId, "Текстовая новость успешно отправлена всем подписчикам.", buttonMainAdmin);
            }
            else
            {
                // Если нет текста или фото
                await _telegramBot.SendTextMessage(chatId, "Ошибка: новость не содержит текста или картинки.",buttonMainAdmin);
            }
        }

        public bool CanHandle(Update update)
        {
            if (update.Message?.Text is not null &&
                update.Message.From is not null &&
                update.UserData.UserChatState == UserChatStates.WaitAdminAddNews ||
                update.Message?.Photo is not null &&
                update.UserData.UserChatState == UserChatStates.WaitAdminAddNews)
            {
                return true;
            }
            return false;
        }
    }