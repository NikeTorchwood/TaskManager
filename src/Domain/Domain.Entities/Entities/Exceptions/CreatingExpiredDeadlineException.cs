using static Domain.Helpers.Domain.ExceptionsMessages.EntitiesExceptionMessages.TaskPointsMessages;

namespace Domain.Entities.Exceptions;

/// <summary>
/// An exception that is thrown if the task deadline is set in the past.
/// </summary>
/// <param name="paramName">The name of the parameter that caused the error.</param>   
public class CreatingExpiredDeadlineException(string paramName)
    : ArgumentException(
        paramName: paramName,
        message: string.Format(ERROR_MESSAGE_DEADLINE_MUST_BE_IN_FUTURE, paramName));