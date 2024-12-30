using static Domain.Helpers.Domain.ExceptionsMessages.EntitiesExceptionMessages.TaskPointsMessages;

namespace Domain.Entities.Exceptions;

/// <summary>
/// An exception that is thrown if you try to complete a failed task.
/// </summary>
public class TaskPointNotStartedException()
    : InvalidOperationException(ERROR_MESSAGE_TASK_POINT_NOT_STARTED);