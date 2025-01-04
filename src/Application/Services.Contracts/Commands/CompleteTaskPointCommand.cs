using MediatR;
using Services.Contracts.Models;

namespace Services.Contracts.Commands;

/// <summary>
/// Represents a command to mark a task point as complete.
/// </summary>
/// <param name="Id">The unique identifier of the task point to mark as complete.</param>
public record CompleteTaskPointCommand(Guid Id) : IRequest<ResultModel<bool>>;