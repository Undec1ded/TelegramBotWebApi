using System;
using System.Linq;
using TestWebTgBot.Models.Dbo;
using TestWebTgBot.Models.TgModels;
using TestWebTgBot.Services.Admin;
using TestWebTgBot.Utils.Buttons;

namespace TestWebTgBot.TgBot.UpdateHandlers.Admin.AddAdmin;

public class AddAdminHandler : IUpdateHandler
{
    private readonly TelegramBot _telegramBot;
    private readonly IAdminService _adminService;

    public AddAdminHandler(TelegramBot telegramBot, IAdminService adminService)
    {
        _telegramBot = telegramBot;
        _adminService = adminService;
    }

    public async Task HandleUpdateAsync(Update update)
    {
        var chatId = update.CallbackQuery!.From.Id;
        var messageId = update.CallbackQuery.Message!.MessageId;

        var buttonAdminStartPanel = AdminButtonsHelper.ButtonGoToAdminStartPanel();

        var randomPassword = GenerateRandomPassword(10);

        var adminPassword = new AdminPasswordEntity
        {
            Password = randomPassword,
            TimeCreated = DateTime.UtcNow
        };

        await _adminService.CreateAdminPasswordAsync(adminPassword);

        await _telegramBot.EditMessageText(chatId, messageId,
            $"Сгенерирован новый пароль для администратора: {randomPassword}",
            buttonAdminStartPanel);
    }

    private string GenerateRandomPassword(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        var random = new Random();
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    public bool CanHandle(Update update)
    {
        if (update.CallbackQuery?.Message is not null && update.CallbackQuery?.From.Id is not null)
        {
            return update.CallbackQuery.Data == "AdminAddAdministrator";
        }
        return false;
    }
}
