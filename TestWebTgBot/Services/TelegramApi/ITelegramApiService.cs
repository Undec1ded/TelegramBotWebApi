namespace TestWebTgBot.Services.TelegramApi;

public interface ITelegramApiService
{
    Task<TResponse> GetMethodAsync<TResponse>(string method);

    Task<TResponse> PostMethodAsync<TResponse, TRequest>(string method, TRequest request);
}