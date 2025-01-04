namespace WebApi.Requests;

/// <summary>
/// Represents the request body used to update the fields of a task point.
/// </summary>
/// <param name="NewDeadline">Gets or sets the new deadline for the task point. If not provided, the deadline will not be updated.</param>
/// <param name="NewTitle">Gets or sets the new title for the task point. If not provided, the title will not be updated.</param>
/// <param name="NewDescription">Gets or sets the new description for the task point. This field is required.</param>
public record UpdateFieldRequest(
    DateTime? NewDeadline = null,
    string? NewTitle = null,
    string NewDescription = null);