using static Domain.Helpers.Domain.ExceptionsMessages.EntitiesExceptionMessages.TaskPointsMessages;

namespace Domain.Entities.Exceptions;

/// <summary>
/// Исключение, которое выбрасывается, если попытаться завершить не стартовавшую задачу.
/// </summary>
internal class TaskPointNotStartedException()
    : InvalidOperationException(ERROR_MESSAGE_TASK_POINT_NOT_STARTED);