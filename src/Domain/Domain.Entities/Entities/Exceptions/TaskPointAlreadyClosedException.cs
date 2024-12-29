using static Domain.Helpers.Domain.ExceptionsMessages.EntitiesExceptionMessages.TaskPointsMessages;

namespace Domain.Entities.Exceptions;

/// <summary>
/// An exception that is thrown if you try to change an already closed task.
/// </summary>
public class TaskPointAlreadyClosedException()
    : InvalidOperationException(ERROR_MESSAGE_TASK_POINT_ALREADY_CLOSED);