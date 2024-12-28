using static Common.Resources.Constants.DescriptionConstants;
using static Domain.Helpers.Domain.ExceptionsMessages.ValueObjectsMessages.DescriptionMessages;

namespace Domain.ValueObjects.Exceptions.DescriptionsExceptions;

/// <summary>
/// Исключение, которое выбрасывается, если длина описания задачи превышает максимальное разрешённое значение.
/// </summary>
/// <param name="valueLength">Длина текущего значения описания задачи.</param>
/// <param name="paramName">Имя параметра, который вызвал ошибку.</param>
public class DescriptionMaxLengthException(int valueLength, string paramName)
    : ArgumentOutOfRangeException(
        paramName: paramName,
        string.Format(ERROR_MESSAGE_DESCRIPTION_LONGER_MAX_LENGTH, DESCRIPTION_MAX_LENGTH, valueLength));