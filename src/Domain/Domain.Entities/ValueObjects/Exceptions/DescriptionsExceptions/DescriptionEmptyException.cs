using static Domain.Helpers.Domain.ExceptionsMessages.ValueObjectsMessages.DescriptionMessages;

namespace Domain.ValueObjects.Exceptions.DescriptionsExceptions;

/// <summary>
/// Исключение, которое выбрасывается, если описание задачи пусто (null или пустая строка).
/// </summary>
/// <param name="paramName">Имя параметра, который вызвал ошибку.</param>
public class DescriptionEmptyException(string paramName)
    : ArgumentNullException(
    paramName: paramName,
    message: ERROR_MESSAGE_DESCRIPTION_EMPTY);