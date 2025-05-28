using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;

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

        var appSettings = configuration.Get<AppSettings>()
            ?? throw new Exception("Ошибка получения настроек");

        foreach (var dir in appSettings.StorageSettings.Directories)
        {
            var files = GetAllFilesRecursively(dir, appSettings.StorageSettings.FileExtensions);

            foreach (var file in files)
                Console.WriteLine($"{file} ({CalculateMD5(file)})");
        }

        // Проверка и создание файла БД
        using var context = new KeeperDbContext();
        context.Database.EnsureCreated();
        Console.WriteLine($"База данных создана: {DbConstants.DbName}");

        // Добавление данных
        var descriptor = new PhotoDescriptor
        {
            Path = "C:\\Dev2"
        };
        context.Add(descriptor);
        //context.SaveChanges();
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

    /// <summary>
    /// Рекурсивный поиск всех файлов в указанной директории
    /// </summary>
    /// <param name="dir"> директория поиска файлов </param>
    /// <param name="searchPattern"> шаблон поиска </param>
    /// <returns></returns>
    private static IEnumerable<string> GetAllFilesRecursively(string dir, string[] searchPattern)
    {
        if (Directory.Exists(dir))
            return Directory.EnumerateFiles(dir, "*", SearchOption.AllDirectories)
                .Where(file => searchPattern.Contains(Path.GetExtension(file), StringComparer.OrdinalIgnoreCase));

        throw new DirectoryNotFoundException($"Директория не найдена: {dir}");
    }

    /// <summary>
    /// Вычисление MD5-хэша файла
    /// </summary>
    /// <param name="filePath"> путь к файлу </param>
    /// <returns></returns>
    private static string CalculateMD5(string filePath)
    {
        if (!File.Exists(filePath))
            throw new FileNotFoundException($"Файл не найден: {filePath}");

        using var stream = File.OpenRead(filePath);
        var hash = MD5.HashData(stream);

        return Convert.ToHexString(hash);
    }

}