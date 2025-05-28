namespace PhotoKeeper;

/// <summary>
/// Настройки хранилища фотографий
/// </summary>
public class StorageSettings
{
    /// <summary>
    /// Директории, где хранятся фото
    /// </summary>
    public required string[] Directories { get; set; }

    /// <summary>
    /// Расширения файлов, которые будут обрабатываться
    /// </summary>
    public required string[] FileExtensions { get; set; }
}
