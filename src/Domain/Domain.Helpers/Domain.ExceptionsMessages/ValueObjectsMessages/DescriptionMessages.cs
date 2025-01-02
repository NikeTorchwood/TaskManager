namespace Domain.Helpers.Domain.ExceptionsMessages.ValueObjectsMessages;

/// <summary>
/// Contains error messages related to the description value object (`Description`).
/// </summary>
public class DescriptionMessages
{
    /// <summary>
    /// Error message if the task description is empty, null, or consists only of spaces.
    /// </summary>
    public const string ERROR_MESSAGE_DESCRIPTION_EMPTY =
        "The task description cannot be null, empty, or consist of spaces.";

    /// <summary>
    /// Error message if the description length is less than the minimum allowed value.
    /// </summary>
    public const string ERROR_MESSAGE_DESCRIPTION_SHORTER_MIN_LENGTH =
        "The task description cannot be shorter than the minimum allowed value. Minimum value - {0}, length value - {1}";

    /// <summary>
    /// Error message if the description length exceeds the maximum allowed value.
    /// </summary>
    public const string ERROR_MESSAGE_DESCRIPTION_LONGER_MAX_LENGTH =
        "The task description cannot be longer than the maximum allowed value. Maximum value - {0}, length value - {1}";
}