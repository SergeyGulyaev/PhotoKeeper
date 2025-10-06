namespace PhotoKeeper;

/// <summary>
/// Дескриптор файла
/// </summary>
public class FileDescriptor
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// Путь к файлу
    /// </summary>
    public required string Path { get; set; }

    /// <summary>
    /// MD5-хэш
    /// </summary>
    public required string Hash { get; set; }
}
