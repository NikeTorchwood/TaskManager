using static Domain.Helpers.Domain.ExceptionsMessages.EntitiesExceptionMessages.TaskPointsMessages;

namespace Domain.Entities.Exceptions;

/// <summary>
/// Исключение, которое выбрасывается,
/// если попытаться изменить свойства запрещенной к редактированию задачи.
/// </summary>
public class TaskPointEditingForbiddenException()
    : InvalidOperationException(ERROR_MESSAGE_TASK_POINT_CLOSED_FOR_EDITING);