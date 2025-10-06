using Microsoft.Extensions.Options;
using System.Security.Cryptography;

namespace PhotoKeeper;

/// <summary>
/// Сервис PhotoKeeper
/// </summary>
/// <param name="appSettings"> настройки приложения </param>
public class PhotoKeeperService(IOptions<AppSettings> appSettings)
{
    private readonly AppSettings settings = appSettings.Value;

    /// <summary>
    /// Создать дескрипторы файлов хранилища
    /// </summary>
    /// <param name="cancellationToken"> токен отмены </param>
    /// <returns></returns>
    public async Task<OperationResult> CreateFileDescriptors(CancellationToken cancellationToken)
    {
        // Проверка и создание файла БД
        using var dbContext = new KeeperDbContext();
        await dbContext.Database.EnsureCreatedAsync(cancellationToken);
        Console.WriteLine($"База данных создана: {DbConstants.DbName}");

        var files = GetStorageFiles();
        // Создание списка дескрипторов
        var fileDescriptors = files.Select(e => new FileDescriptor
        {
            Path = e,
            Hash = CalculateMD5(e)
        });

        await dbContext.AddRangeAsync(fileDescriptors, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new OperationResult
        { 
            Total = files.Count,
            Success = files,
            Failed = []
        };
    }

    /// <summary>
    /// Получить список всех файлов хранилища
    /// </summary>
    /// <returns></returns>
    private List<string> GetStorageFiles()
    {
        var storageFiles = new List<string>();

        // обход директорий хранилища указанных в настройках
        foreach (var dir in settings.StorageSettings.Directories)
        {
            var filePaths = GetDirFiles(dir, settings.StorageSettings.FileExtensions);
            storageFiles.AddRange(filePaths);
        }

        return storageFiles;
    }

    /// <summary>
    /// Рекурсивный поиск всех файлов в указанной директории
    /// </summary>
    /// <param name="dir"> директория поиска файлов </param>
    /// <param name="searchPattern"> шаблон поиска </param>
    /// <returns></returns>
    private static IEnumerable<string> GetDirFiles(string dir, string[] searchPattern)
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
