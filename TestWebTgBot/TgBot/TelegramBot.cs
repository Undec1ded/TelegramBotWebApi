using TestWebTgBot.Models.Requests;
using TestWebTgBot.Models.Response;
using TestWebTgBot.Models.TgModels;
using TestWebTgBot.Services.TelegramApi;
using TestWebTgBot.Utils;

namespace TestWebTgBot.TgBot;

public class TelegramBot
{
    private readonly ITelegramApiService _telegramApiService;
    private string _token;
    private int _biggestLastUpdate;
    private IUpdateListener? _updateListener;
    
    public TelegramBot(ITelegramApiService telegramApiService)
    {
        _telegramApiService = telegramApiService;
    }
    public async Task Run(CancellationToken clt)
    {
        var data = await GetMe();
        while (!clt.IsCancellationRequested)
        {
            var updateResponse = await GetUpdates();
            if (updateResponse.Ok && updateResponse.Result is not null)
            {
                var tasks = new List<Task>();
                foreach (var update in updateResponse.Result)
                {
                    if (update.UpdateId > _biggestLastUpdate)
                    {
                        _biggestLastUpdate = update.UpdateId;
                    }
                    if (_updateListener is not null)
                    {
                        tasks.Add(_updateListener.OnUpdate(update, this));
                    }
                }
                await Task.WhenAll(tasks);
            }

        }
    }
    //telegram
    public async Task SendTextMessage(long chatId, string message, InlineKeyboardMarkup? inlineReplyMarkup = null, ReplyKeyboardMarkup? replyKeyboardMarkup = null)
    {
        var request = new SendMessageRequest()
        {
            BusinessConnectionId = null,
            ChatId = chatId,
            MessageThreadId = null,
            Text = message,
            InlineReplyMarkup = inlineReplyMarkup
        };
        var response = await _telegramApiService.PostMethodAsync<SendMessageResponse, SendMessageRequest>("sendMessage", request);
    }
    public async Task SendPhoto(long chatId, string photo, string? caption = null)
    {
        var request = new SendPhotoRequest()
        {
            ChatId = chatId,
            Photo = photo,
            Caption = caption
        };
        var response = await _telegramApiService.PostMethodAsync<SendPhotoResponse, SendPhotoRequest>("sendPhoto", request);
    }
    public async Task DeleteMessage(long chatId, int messageId)
    {
        var request = new DeleteMessageRequest()
        {
            ChatId = chatId,
            MessageId = messageId
        };
        var response = await _telegramApiService.PostMethodAsync<DeleteMessageResponse, DeleteMessageRequest>("deleteMessage", request);
        if (!response.Ok || (response.Result.HasValue && !response.Result.Value))
        {
            throw new Exception($"Failed to delete message: {response.Result?.ToString()}");
        }
        Console.WriteLine($"Message deleted");
    }
    public async Task DeleteMessages(long chatId, List<int> messageIds)
    {
        var request = new DeleteMessagesRequest()
        {
            ChatId = chatId,
            MessageIds = messageIds
        };
        var response = await _telegramApiService.PostMethodAsync<DeleteMessagesResponse, DeleteMessagesRequest>("deleteMessages", request);
    }

    public async Task EditMessageReplyMarkup(long chatId, int messageId, InlineKeyboardMarkup nReplyKeyboardMarkup)
    {
        var request = new EditMessageReplyMarkupRequest()
        {
            ChatId = chatId,
            MessageId = messageId,
            ReplyMarkup = nReplyKeyboardMarkup
        };
        var response =
            await _telegramApiService.PostMethodAsync<EditMessageReplyMarkupResponse, EditMessageReplyMarkupRequest>(
                "editMessageReplyMarkup", request);
    }
    public async Task EditMessageText(long chatId, int messageId, string text, InlineKeyboardMarkup? nInlineReplyMarkup = null)
    {
        var request = new EditMessageTextRequest()
        {
            ChatId = chatId,
            MessageId = messageId,
            Text = text,
            ReplyMarkup = nInlineReplyMarkup
        };
        var response =
            await _telegramApiService.PostMethodAsync<EditMessageTextResponse, EditMessageTextRequest>(
                "editMessageText", request);
    }

    public void AddUpdateListener(IUpdateListener updateListener)
    {
        if(_updateListener is not null)
        {
            throw new Exception("Already taking");
        }
        _updateListener = updateListener;
    }
    private async Task<GetMeResponse> GetMe()
    {
        var data = await _telegramApiService.GetMethodAsync<GetMeResponse>("getMe");
        return data;
    }

    private async Task<GetUpdatesResponse> GetUpdates()
    {
        var request = new GetUpdatesRequest()
        {
            Offset = _biggestLastUpdate + 1,
            Limit = 50,
            Timeout = 5
        };
        var data = await _telegramApiService.PostMethodAsync<GetUpdatesResponse , GetUpdatesRequest>("getUpdates" , request);
        
        return data;
    }
    
}
