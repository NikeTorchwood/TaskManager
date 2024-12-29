namespace Domain.Helpers.Domain.ExceptionsMessages.ValueObjectsMessages;

/// <summary>
/// Contains error messages related to the title value object (`Title`).
/// </summary>
public static class TitleMessages
{
    /// <summary>
    /// Error message if the task title is empty, null, or consists only of spaces.
    /// </summary>
    public const string ERROR_MESSAGE_TITLE_EMPTY =
        "The task title cannot be null, empty, or consist of spaces.";

    /// <summary>
    /// Error message if the header length is less than the minimum allowed value.
    /// </summary>
    public const string ERROR_MESSAGE_TITLE_SHORTER_MIN_LENGTH =
        "The task title cannot be shorter than the minimum allowed value. Minimum value - {0}, length value - {1}";

    /// <summary>
    /// Error message if the header length exceeds the maximum allowed value.
    /// </summary>
    public const string ERROR_MESSAGE_TITLE_LONGER_MAX_LENGTH =
        "The task title cannot be longer than the maximum allowed value. Maximum value - {0}, length value - {1}";
}