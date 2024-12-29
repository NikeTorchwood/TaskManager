using static Domain.Helpers.Domain.ExceptionsMessages.EntitiesExceptionMessages.TaskPointsMessages;

namespace Domain.Entities.Exceptions;

/// <summary>
/// The exception that is thrown
/// if you try to change the properties of a task that is prohibited from editing.
/// </summary>
public class TaskPointEditingForbiddenException()
    : InvalidOperationException(ERROR_MESSAGE_TASK_POINT_CLOSED_FOR_EDITING);