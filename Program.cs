using Microsoft.Extensions.Configuration;

namespace PhotoKeeper;

/// <summary>
/// Приложение
/// </summary>
public class Program
{
    /// <summary>
    /// конфигурация приложения
    /// </summary>
    public static IConfigurationRoot Configuration { get; private set; } = null!;

    /*
    /// <summary>
    /// провайдер сервисов
    /// </summary>
        public static ServiceProvider ServiceProvider { get; private set; } = null!;
    */

    /// <summary>
    /// Точка входа
    /// </summary>
    static void Main()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        var storageSettings = configuration.Get<AppSettings>();

        // Проверка и создание файла БД
        using var context = new KeeperDbContext();
        context.Database.EnsureCreated();
        Console.WriteLine($"База данных {DbConstants.DbName} создана");

        // Добавление данных
        var descriptor = new PhotoDescriptor
        {
            Path = "C:\\Dev2"
        };
        context.Add(descriptor);
        context.SaveChanges();
        Console.WriteLine("Всё ок!");


        //try
        //{
        //    ServiceProvider = ConfigureServices(new WebHostEnvironment(env));
        //}
        //catch (Exception ex)
        //{
        //    writer.WriteLineError("Ошибка настройки сервисов");
        //    writer.WriteException(ex);
        //    throw;
        //}

        // вызываем сервис
        //Run();
    }

}