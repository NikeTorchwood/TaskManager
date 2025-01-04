namespace Common.Resources.Constants;

/// <summary>
/// Constants related to task title length constraints.
/// </summary>
/// <remarks>
/// This class contains constants defining the minimum and maximum length for a task title.
/// These values are used for validating data when creating or updating tasks.
/// </remarks>
public static class TitleConstants
{
    /// <summary>
    /// The maximum allowed length for a task title.
    /// </summary>
    /// <remarks>
    /// This is the maximum number of characters that a task title can have.
    /// </remarks>
    public const int TITLE_MAX_LENGTH = 128;

    /// <summary>
    /// The minimum required length for a task title.
    /// </summary>
    /// <remarks>
    /// This is the minimum number of characters that a task title must contain.
    /// </remarks>
    public const int TITLE_MIN_LENGTH = 2;
}