namespace Domain.Helpers.Domain.ExceptionsMessages.ValueObjectsMessages;

/// <summary>
/// Содержит сообщения об ошибках, связанных с объектом значения заголовка (`Title`).
/// </summary>
public static class TitleMessages
{
    /// <summary>
    /// Сообщение об ошибке, если заголовок задачи пустой, равен null или состоит только из пробелов.
    /// </summary>
    public const string ERROR_MESSAGE_TITLE_EMPTY =
        "The task title cannot be null, empty, or consist of spaces.";

    /// <summary>
    /// Сообщение об ошибке, если длина заголовка меньше минимально допустимого значения.
    /// </summary>
    public const string ERROR_MESSAGE_TITLE_SHORTER_MIN_LENGTH =
        "The task title cannot be shorter than the minimum allowed value.";

    /// <summary>
    /// Сообщение об ошибке, если длина заголовка превышает максимально допустимое значение.
    /// </summary>
    public const string ERROR_MESSAGE_TITLE_LONGER_MAX_LENGTH =
        "The task title cannot be longer than the maximum allowed value.";
}