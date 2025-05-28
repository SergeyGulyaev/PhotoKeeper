namespace PhotoKeeper;

/// <summary>
/// Настройки приложения
/// </summary>
public class AppSettings
{
    /// <summary>
    /// Настройки хранилища фотографий
    /// </summary>
    public required StorageSettings StorageSettings { get; set; }
}
