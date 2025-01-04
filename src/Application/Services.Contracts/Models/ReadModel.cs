using Domain.Entities;
using Domain.Entities.Enums;

namespace Services.Contracts.Models;

/// <summary>
/// Represents a read-only model for <see cref="TaskPoint"/> entities.
/// </summary>
/// <param name="Id">The unique identifier of the task point.</param>
/// <param name="Title">The title of the task point.</param>
/// <param name="Description">The description of the task point.</param>
/// <param name="Status">The current status of the task point.</param>
/// <param name="IsDeleted">Indicates whether the task point is deleted.</param>
/// <param name="CreatedAt">The creation date and time of the task point.</param>
/// <param name="Deadline">The deadline date and time of the task point.</param>
/// <param name="StartedAt">The date and time when the task point was started (optional).</param>
/// <param name="ClosedAt">The date and time when the task point was closed (optional).</param>
public record ReadModel(
    Guid Id,
    string Title,
    string Description,
    TaskPointStatuses Status,
    bool IsDeleted,
    DateTime CreatedAt,
    DateTime Deadline,
    DateTime? StartedAt,
    DateTime? ClosedAt);