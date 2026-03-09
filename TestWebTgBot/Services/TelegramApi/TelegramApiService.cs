using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TestWebTgBot.Services.TelegramApi;

public class TelegramApiService : ITelegramApiService
{
    private readonly HttpClient _httpClient;
    private string _baseUrl;

    public TelegramApiService(IConfiguration configuration , IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient();
        _baseUrl = $"https://api.telegram.org/bot{configuration["Telegram:BotToken"]}/";
    }
    public async Task<TResponse> GetMethodAsync<TResponse>(string method)
    {
        var msg = new HttpRequestMessage();
        msg.Method = HttpMethod.Get;
        msg.RequestUri = GetMethodUri(method);
        
        var response = await _httpClient.SendAsync(msg);
        var responseContent = await response.Content.ReadAsStringAsync();
        var responseData = JsonSerializer.Deserialize<TResponse>(responseContent);

        if (responseData is null)
        {
            throw new Exception("Error1");
        }
        
        return responseData;
    }

    public async Task<TResponse> PostMethodAsync<TResponse, TRequest>(string method, TRequest request)
    {
        var msg = new HttpRequestMessage();
        msg.Method = HttpMethod.Post;
        msg.RequestUri = GetMethodUri(method);
        var serializedObject = JsonSerializer.Serialize(request);
        msg.Content = new StringContent(serializedObject, Encoding.UTF8, "application/json");
        var response = await _httpClient.SendAsync(msg);
        var responseContetn = await response.Content.ReadAsStringAsync();
        
        if (response.StatusCode != HttpStatusCode.OK)
        {
            throw new Exception($"Error0: {response.StatusCode}, {responseContetn}, {serializedObject}");
        }

        var responseData = JsonSerializer.Deserialize<TResponse>(responseContetn);

        if (responseData is null)
        {
            throw new Exception("Error1");
        }
        
        return responseData;
    }
    private Uri GetMethodUri(string method)
    {
        return new Uri(_baseUrl + method);
    }
}
