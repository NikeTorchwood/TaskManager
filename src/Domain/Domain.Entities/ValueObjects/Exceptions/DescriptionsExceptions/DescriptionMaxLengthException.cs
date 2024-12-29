using static Common.Resources.Constants.DescriptionConstants;
using static Domain.Helpers.Domain.ExceptionsMessages.ValueObjectsMessages.DescriptionMessages;

namespace Domain.ValueObjects.Exceptions.DescriptionsExceptions;

/// <summary>
/// An exception that is thrown if the length of the task description exceeds the maximum allowed value.
/// </summary>
/// <param name="valueLength">The length of the current value of the task description.</param>
/// <param name="paramName">The name of the parameter that caused the error.</param>
public class DescriptionMaxLengthException(int valueLength, string paramName)
    : ArgumentOutOfRangeException(
        paramName: paramName,
        string.Format(ERROR_MESSAGE_DESCRIPTION_LONGER_MAX_LENGTH, DESCRIPTION_MAX_LENGTH, valueLength));