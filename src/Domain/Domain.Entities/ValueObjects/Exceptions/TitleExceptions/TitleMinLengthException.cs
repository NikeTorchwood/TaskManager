using static Common.Resources.Constants.TitleConstants;
using static Domain.Helpers.Domain.ExceptionsMessages.ValueObjectsMessages.TitleMessages;

namespace Domain.ValueObjects.Exceptions.TitleExceptions;

/// <summary>
/// An exception that is thrown if the length of the task header is less than the minimum allowed value.
/// </summary>
/// <param name="valueLength">The length of the current issue header value.</param>
/// <param name="paramName">The name of the parameter that caused the error.</param
public class TitleMinLengthException(int valueLength, string paramName)
    : ArgumentOutOfRangeException(
        paramName: paramName,
        string.Format(ERROR_MESSAGE_TITLE_SHORTER_MIN_LENGTH, TITLE_MIN_LENGTH, valueLength));