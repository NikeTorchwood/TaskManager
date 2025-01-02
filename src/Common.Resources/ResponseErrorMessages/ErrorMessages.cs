namespace Common.Resources.ResponseErrorMessages;
public class ErrorMessages
{
    public const string ERROR_MESSAGE_TASK_NOT_FOUND = "Task point not found";
    public const string ERROR_MESSAGE_TASK_ALREADY_CLOSED = "Task point already closed";
    public const string ERROR_MESSAGE_CANT_COMPLETE_CLOSED_TASK = "Cant complete closed task point.";
    public const string ERROR_MESSAGE_CANT_COMPLETE_NOT_OPENED_TASK = "Cant complete not opened task point";
    public const string ERROR_MESSAGE_INVALID_DATA = "Invalid task data.";
    public const string ERROR_MESSAGE_CANT_START_CLOSED_TASK = "Cant start closed task point.";
    public const string ERROR_MESSAGE_TASK_ALREADY_STARTED = "Task point already started";
    public const string ERROR_MESSAGE_DEADLINE_MUST_BE_IN_FUTURE = "The deadline should be in the future time.";

    public const string ERROR_MESSAGE_TITLE_SHORTER_MIN_LENGTH =
        "The task title cannot be shorter than the minimum allowed value.";

    public const string ERROR_MESSAGE_TITLE_LONGER_MAX_LENGTH = 
        "The task title cannot be longer than the maximum allowed value.";

    public const string ERROR_MESSAGE_DESCRIPTION_SHORTER_MIN_LENGTH =
        "The task description cannot be shorter than the minimum allowed value.";

    public const string ERROR_MESSAGE_DESCRIPTION_LONGER_MAX_LENGTH =
        "The task description cannot be longer than the maximum allowed value.";
}