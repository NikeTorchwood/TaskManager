using static Domain.Helpers.Domain.ExceptionsMessages.EntitiesExceptionMessages.TaskPointsMessages;

namespace Domain.Entities.Exceptions;

/// <summary>
/// Исключение, которое выбрасывается, если попытаться изменить уже закрытую задачу.
/// </summary>
public class TaskPointAlreadyClosedException()
    : InvalidOperationException(ERROR_MESSAGE_TASK_POINT_ALREADY_CLOSED);