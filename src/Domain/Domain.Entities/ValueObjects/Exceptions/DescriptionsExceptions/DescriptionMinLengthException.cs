using static Common.Resources.Constants.DescriptionConstants;
using static Domain.Helpers.Domain.ExceptionsMessages.ValueObjectsMessages.DescriptionMessages;

namespace Domain.ValueObjects.Exceptions.DescriptionsExceptions;

/// <summary>
/// Исключение, которое выбрасывается, если длина описания задачи меньше минимально разрешённого значения.
/// </summary>
/// <param name="valueLength">Длина текущего значения описания.</param>
/// <param name="paramName">Имя параметра, который вызвал ошибку.</param
public class DescriptionMinLengthException(int valueLength, string paramName)
    : ArgumentOutOfRangeException(
        paramName: paramName,
        string.Format(ERROR_MESSAGE_DESCRIPTION_LONGER_MIN_LENGTH, DESCRIPTION_MIN_LENGTH, valueLength));