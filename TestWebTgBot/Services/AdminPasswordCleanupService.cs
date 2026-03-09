using TestWebTgBot.Services.Admin;

namespace TestWebTgBot.Services;

public class AdminPasswordCleanupService : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly TimeSpan _cleanupInterval = TimeSpan.FromMinutes(5);
    private readonly TimeSpan _passwordLifetime = TimeSpan.FromMinutes(10);

    public AdminPasswordCleanupService(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _serviceScopeFactory.CreateScope();

                var adminService = scope.ServiceProvider.GetRequiredService<IAdminService>();

                var nowUtc = DateTime.UtcNow;

                var allPasswords = await adminService.GetAllAdminPasswordsAsync();

                var expiredPasswords = allPasswords
                    .Where(password => password.TimeCreated < nowUtc.Subtract(_passwordLifetime))
                    .ToList();

                foreach (var expiredPassword in expiredPasswords)
                {
                    await adminService.DeletePasswordByDateAsync(expiredPassword.TimeCreated);
                }

                Console.WriteLine($"Удалено {expiredPasswords.Count} устаревших паролей.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка в AdminPasswordCleanupService: {ex.Message}");
            }

            await Task.Delay(_cleanupInterval, stoppingToken);
        }
    }
}