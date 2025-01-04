using MediatR;
using Services.Contracts.Models;

namespace Services.Contracts.Commands;

/// <summary>
/// Represents a command to create a new task point.
/// </summary>
/// <param name="Title">The title of the task point.</param>
/// <param name="Description">The description of the task point.</param>
/// <param name="Deadline">The deadline by which the task point must be completed.</param>
/// <param name="IsStarted">Indicates whether the task point is already started.</param>
public record CreateTaskPointCommand(
    string Title,
    string Description,
    DateTime Deadline,
    bool IsStarted) : IRequest<ResultModel<ReadModel>>;