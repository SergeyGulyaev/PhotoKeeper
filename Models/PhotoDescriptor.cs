namespace PhotoKeeper;

/// <summary>
/// Дескриптор фотографии
/// </summary>
public class PhotoDescriptor
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// Путь к файлу
    /// </summary>
    public required string Path { get; set; }
}
