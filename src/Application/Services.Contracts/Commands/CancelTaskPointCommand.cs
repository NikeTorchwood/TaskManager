using MediatR;
using Services.Contracts.Models;

namespace Services.Contracts.Commands;

/// <summary>
/// Represents a command to cancel a task point.
/// </summary>
/// <param name="Id">The unique identifier of the task point to cancel.</param>
public record CancelTaskPointCommand(Guid Id)
    : IRequest<ResultModel<bool>>;