using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace PhotoKeeper;

/// <summary>
/// Стартовый класс приложения
/// </summary>
public class Program
{
    /// <summary>
    /// Точка входа
    /// </summary>
    static async Task Main(string[] args)
    {
        // создание хоста, регистрация настроек и сервисов
        var host = Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
                services.Configure<AppSettings>(context.Configuration);
                services.AddSingleton<PhotoKeeperService>();
            })
            .Build();

        // получение сервисов через DI
        var appSettings = host.Services.GetRequiredService<IOptions<AppSettings>>().Value;
        var photoKeeperService = host.Services.GetRequiredService<PhotoKeeperService>();

        using var cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = cancellationTokenSource.Token;

        var result = await photoKeeperService.CreateFileDescriptors(cancellationToken);

        foreach (var item in result.Success)
            Console.WriteLine($"{item}");

        Console.WriteLine("OK!");
    }
}