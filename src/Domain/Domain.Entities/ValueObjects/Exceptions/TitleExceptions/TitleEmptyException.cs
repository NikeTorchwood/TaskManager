using static Domain.Helpers.Domain.ExceptionsMessages.ValueObjectsMessages.TitleMessages;

namespace Domain.ValueObjects.Exceptions.TitleExceptions;

/// <summary>
/// An exception that is thrown if the task title is empty (null or an empty string).
/// </summary>
/// <param name="paramName">The name of the parameter that caused the error.</param>
public class TitleEmptyException(string paramName) : ArgumentNullException(
    paramName: paramName,
    message: ERROR_MESSAGE_TITLE_EMPTY);