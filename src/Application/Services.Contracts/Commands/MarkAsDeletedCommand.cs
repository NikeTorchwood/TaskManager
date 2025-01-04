using MediatR;
using Services.Contracts.Models;

namespace Services.Contracts.Commands;

/// <summary>
/// Represents a command to mark a task point as deleted.
/// </summary>
/// <param name="Id">The unique identifier of the task point to mark as deleted.</param>
public record MarkAsDeletedCommand(Guid Id)
    : IRequest<ResultModel<bool>>;