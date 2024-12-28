namespace Domain.Entities.Enums;

/// <summary>
/// Статусы задачи в системе управления задачами.
/// </summary>
public enum TaskPointStatuses
{
    /// <summary>
    /// Задача была создана, но ещё не начата.
    /// </summary>
    Created = 0,

    /// <summary>
    /// Задача в процессе выполнения.
    /// </summary>
    InProgress = 1,

    /// <summary>
    /// Задача была успешно завершена.
    /// </summary>
    Completed = 2,

    /// <summary>
    /// Задача была отменена.
    /// </summary>
    Cancelled = 3
}