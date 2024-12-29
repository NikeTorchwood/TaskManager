namespace Domain.Helpers.Domain.ExceptionsMessages.EntitiesExceptionMessages;

/// <summary>
/// Contains task-related error messages (`TaskPoint') used in exceptions.
/// </summary>
public static class TaskPointsMessages
{
    /// <summary>
    /// Error message if the deadline for the task is specified in the past.
    /// </summary>
    public const string ERROR_MESSAGE_DEADLINE_MUST_BE_IN_FUTURE =
        "The {0} parameter has an invalid value. The deadline should be in the future time.";

    /// <summary>
    /// Error message if the task has already been closed or completed.
    /// </summary>
    public const string ERROR_MESSAGE_TASK_POINT_ALREADY_CLOSED =
        "The task point has already been closed or completed.";

    /// <summary>
    /// Error message if the task is closed for editing because it has been canceled or completed.
    /// </summary>
    public const string ERROR_MESSAGE_TASK_POINT_CLOSED_FOR_EDITING =
        "The task point is closed for editing because it has been cancelled or completed.";

    /// <summary>
    /// Error message if the task has not been started yet.
    /// </summary>
    public const string ERROR_MESSAGE_TASK_POINT_NOT_STARTED =
        "The task point was not started.";
}
