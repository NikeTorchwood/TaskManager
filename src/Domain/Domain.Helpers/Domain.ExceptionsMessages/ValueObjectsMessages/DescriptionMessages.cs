namespace Domain.Helpers.Domain.ExceptionsMessages.ValueObjectsMessages;

/// <summary>
/// Содержит сообщения об ошибках, связанных с объектом значения описания (`Description`).
/// </summary>
public class DescriptionMessages
{
    /// <summary>
    /// Сообщение об ошибке, если описание задачи пустое, равно null или состоит только из пробелов.
    /// </summary>
    public const string ERROR_MESSAGE_DESCRIPTION_EMPTY =
        "The task description cannot be null, empty, or consist of spaces.";

    /// <summary>
    /// Сообщение об ошибке, если длина описания меньше минимально допустимого значения.
    /// </summary>
    public const string ERROR_MESSAGE_DESCRIPTION_LONGER_MIN_LENGTH =
        "The task description cannot be shorter than the minimum allowed value.";

    /// <summary>
    /// Сообщение об ошибке, если длина описания превышает максимально допустимое значение.
    /// </summary>
    public const string ERROR_MESSAGE_DESCRIPTION_LONGER_MAX_LENGTH =
        "The task description cannot be longer than the maximum allowed value.";
}