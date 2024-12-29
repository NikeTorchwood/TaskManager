using static Common.Resources.Constants.TitleConstants;
using static Domain.Helpers.Domain.ExceptionsMessages.ValueObjectsMessages.TitleMessages;

namespace Domain.ValueObjects.Exceptions.TitleExceptions;

/// <summary>
/// An exception that is thrown if the length of the task header exceeds the maximum allowed value.
/// </summary>
/// <param name="valueLength">The length of the current issue header value.</param>
/// <param name="paramName">The name of the parameter that caused the error.</param>
public class TitleMaxLengthException(int valueLength, string paramName)
    : ArgumentOutOfRangeException(
    paramName: paramName,
    string.Format(ERROR_MESSAGE_TITLE_LONGER_MAX_LENGTH, TITLE_MAX_LENGTH, valueLength));