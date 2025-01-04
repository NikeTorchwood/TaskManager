namespace WebApi.Requests;

/// <summary>
/// Represents the request body used to create a new task point.
/// </summary>
/// <param name="Title"> Gets or sets the title of the task point.</param>
/// <param name="Description">Gets or sets the description of the task point.</param>
/// <param name="Deadline">Gets or sets the deadline of the task point.</param>
/// <param name="IsStarted">Gets or sets a value indicating whether the task point is started.</param>
public record CreatingTaskPointRequest(
    string Title,
    string Description,
    DateTime Deadline,
    bool IsStarted);