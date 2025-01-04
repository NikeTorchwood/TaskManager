namespace Common.Resources.ResponseErrorMessages;

/// <summary>
/// Class containing constant error messages for task points.
/// </summary>
/// <remarks>
/// This class serves as a centralized repository for all error messages used across the application.
/// These constants are used to provide standardized error responses for task point operations.
/// </remarks>
public class ErrorMessages
{
    /// <summary>
    /// The error message when the task point is not found.
    /// </summary>
    public const string ERROR_MESSAGE_TASK_NOT_FOUND = "Task point not found.";

    /// <summary>
    /// The error message when the task point has already been deleted.
    /// </summary>
    public const string ERROR_MESSAGE_TASK_WAS_DELETED = "Task point was deleted.";

    /// <summary>
    /// The error message when the task point has already been closed.
    /// </summary>
    public const string ERROR_MESSAGE_TASK_ALREADY_CLOSED = "Task point already closed.";

    /// <summary>
    /// The error message when an attempt is made to complete a closed task point.
    /// </summary>
    public const string ERROR_MESSAGE_CANT_COMPLETE_CLOSED_TASK = "Cant complete closed task point.";

    /// <summary>
    /// The error message when an attempt is made to complete a task point that has not been started.
    /// </summary>
    public const string ERROR_MESSAGE_CANT_COMPLETE_NOT_OPENED_TASK = "Cant complete not opened task point";

    /// <summary>
    /// The error message when the provided task data is invalid.
    /// </summary>
    public const string ERROR_MESSAGE_INVALID_DATA = "Invalid task data.";

    /// <summary>
    /// The error message when an attempt is made to start a closed task point.
    /// </summary>
    public const string ERROR_MESSAGE_CANT_START_CLOSED_TASK = "Cant start closed task point.";

    /// <summary>
    /// The error message when the task point has already been started.
    /// </summary>
    public const string ERROR_MESSAGE_TASK_ALREADY_STARTED = "Task point already started.";

    /// <summary>
    /// The error message when the deadline is in the past.
    /// </summary>
    public const string ERROR_MESSAGE_DEADLINE_MUST_BE_IN_FUTURE = "The deadline should be in the future time.";

    /// <summary>
    /// The error message when the task title is shorter than the minimum allowed length.
    /// </summary>
    public const string ERROR_MESSAGE_TITLE_SHORTER_MIN_LENGTH =
        "The task title cannot be shorter than the minimum allowed value.";

    /// <summary>
    /// The error message when the task title is longer than the maximum allowed length.
    /// </summary>
    public const string ERROR_MESSAGE_TITLE_LONGER_MAX_LENGTH =
        "The task title cannot be longer than the maximum allowed value.";

    /// <summary>
    /// The error message when the task description is shorter than the minimum allowed length.
    /// </summary>
    public const string ERROR_MESSAGE_DESCRIPTION_SHORTER_MIN_LENGTH =
        "The task description cannot be shorter than the minimum allowed value.";

    /// <summary>
    /// The error message when the task description is longer than the maximum allowed length.
    /// </summary>
    public const string ERROR_MESSAGE_DESCRIPTION_LONGER_MAX_LENGTH =
        "The task description cannot be longer than the maximum allowed value.";
}