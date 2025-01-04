using MediatR;
using Services.Contracts.Models;

namespace Services.Contracts.Commands;

/// <summary>
/// Represents a command to start a task point.
/// </summary>
/// <param name="Id">The unique identifier of the task point to start.</param>
public record StartTaskPointCommand(Guid Id)
    : IRequest<ResultModel<bool>>;