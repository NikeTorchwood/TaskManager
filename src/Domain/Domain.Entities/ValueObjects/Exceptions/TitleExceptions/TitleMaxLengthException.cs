using static Common.Resources.Constants.TitleConstants;
using static Domain.Helpers.Domain.ExceptionsMessages.ValueObjectsMessages.TitleMessages;

namespace Domain.ValueObjects.Exceptions.TitleExceptions;

/// <summary>
/// Исключение, которое выбрасывается, если длина заголовка задачи превышает максимальное разрешённое значение.
/// </summary>
/// <param name="valueLength">Длина текущего значения заголовка задачи.</param>
/// <param name="paramName">Имя параметра, который вызвал ошибку.</param>
public class TitleMaxLengthException(int valueLength, string paramName)
    : ArgumentOutOfRangeException(
    paramName: paramName,
    string.Format(ERROR_MESSAGE_TITLE_LONGER_MAX_LENGTH, TITLE_MAX_LENGTH, valueLength));