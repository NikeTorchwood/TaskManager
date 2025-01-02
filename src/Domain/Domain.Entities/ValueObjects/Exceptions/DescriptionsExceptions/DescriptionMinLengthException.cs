using static Common.Resources.Constants.DescriptionConstants;
using static Domain.Helpers.Domain.ExceptionsMessages.ValueObjectsMessages.DescriptionMessages;

namespace Domain.ValueObjects.Exceptions.DescriptionsExceptions;

/// <summary>
/// An exception that is thrown if the length of the task description is less than the minimum allowed value.
/// </summary>
/// <param name="valueLength">The length of the current description value.</param>
/// <param name="paramName">The name of the parameter that caused the error.</param
public class DescriptionMinLengthException(int valueLength, string paramName)
    : ArgumentOutOfRangeException(
        paramName: paramName,
        string.Format(ERROR_MESSAGE_DESCRIPTION_SHORTER_MIN_LENGTH, DESCRIPTION_MIN_LENGTH, valueLength));