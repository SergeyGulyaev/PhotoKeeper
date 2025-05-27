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
    /// Тест
    /// </summary>
    public required string Test { get; set; }
}
