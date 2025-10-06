namespace PhotoKeeper;

/// <summary>
/// Результат операции
/// </summary>
public class OperationResult
{
    /// <summary>
    /// Общее количество элементов
    /// </summary>
    public int Total { get; set; }

    /// <summary>
    /// Успешно
    /// </summary>
    public required List<string> Success { get; set; }

    /// <summary>
    /// Провалено
    /// </summary>
    public required List<string> Failed { get; set; }
}
