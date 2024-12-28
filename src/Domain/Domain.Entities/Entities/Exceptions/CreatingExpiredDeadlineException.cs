using static Domain.Helpers.Domain.ExceptionsMessages.EntitiesExceptionMessages.TaskPointsMessages;

namespace Domain.Entities.Exceptions;

/// <summary>
/// Исключение, которое выбрасывается, если дедлайн задачи установлен в прошлом.
/// </summary>
/// <param name="paramName">Имя параметра, который вызвал ошибку.</param>   
public class CreatingExpiredDeadlineException(string paramName)
    : ArgumentException(
        paramName: paramName,
        message: string.Format(ERROR_MESSAGE_DEADLINE_MUST_BE_IN_FUTURE, paramName));