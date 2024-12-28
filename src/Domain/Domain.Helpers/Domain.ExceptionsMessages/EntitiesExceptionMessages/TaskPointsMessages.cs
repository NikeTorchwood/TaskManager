namespace Domain.Helpers.Domain.ExceptionsMessages.EntitiesExceptionMessages;

/// <summary>
/// Содержит сообщения об ошибках, связанных с задачами (`TaskPoint`), используемые в исключениях.
/// </summary>
public static class TaskPointsMessages
{
    /// <summary>
    /// Сообщение об ошибке, если крайний срок задачи указан в прошлом.
    /// </summary>
    public const string ERROR_MESSAGE_DEADLINE_MUST_BE_IN_FUTURE =
        "The {0} parameter has an invalid value. The deadline should be in the future time.";

    /// <summary>
    /// Сообщение об ошибке, если задача уже закрыта или завершена.
    /// </summary>
    public const string ERROR_MESSAGE_TASK_POINT_ALREADY_CLOSED =
        "The task point has already been closed or completed.";

    /// <summary>
    /// Сообщение об ошибке, если задача закрыта для редактирования, так как она была отменена или завершена.
    /// </summary>
    public const string ERROR_MESSAGE_TASK_POINT_CLOSED_FOR_EDITING =
        "The task point is closed for editing because it has been cancelled or completed.";

    /// <summary>
    /// Сообщение об ошибке, если задача ещё не была начата.
    /// </summary>
    public const string ERROR_MESSAGE_TASK_POINT_NOT_STARTED =
        "The task point was not started.";
}
