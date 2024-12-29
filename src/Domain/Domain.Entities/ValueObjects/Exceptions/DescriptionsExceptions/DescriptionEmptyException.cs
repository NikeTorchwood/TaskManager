using static Domain.Helpers.Domain.ExceptionsMessages.ValueObjectsMessages.DescriptionMessages;

namespace Domain.ValueObjects.Exceptions.DescriptionsExceptions;

/// <summary>
/// An exception that is thrown if the task description is empty (null or an empty string).
/// </summary>
/// <param name="paramName">The name of the parameter that caused the error.</param>
public class DescriptionEmptyException(string paramName)
    : ArgumentNullException(
    paramName: paramName,
    message: ERROR_MESSAGE_DESCRIPTION_EMPTY);