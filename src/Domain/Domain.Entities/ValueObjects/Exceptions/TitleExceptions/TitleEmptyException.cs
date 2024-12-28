using static Domain.Helpers.Domain.ExceptionsMessages.ValueObjectsMessages.TitleMessages;

namespace Domain.ValueObjects.Exceptions.TitleExceptions;

/// <summary>
/// Исключение, которое выбрасывается, если заголовок задачи пуст (null или пустая строка).
/// </summary>
/// <param name="paramName">Имя параметра, который вызвал ошибку.</param>
public class TitleEmptyException(string paramName) : ArgumentNullException(
    paramName: paramName,
    message: ERROR_MESSAGE_TITLE_EMPTY);