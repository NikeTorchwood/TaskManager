using MediatR;
using Services.Contracts.Models;

namespace Services.Contracts.Commands;

public record CreateTaskPointCommand(
    string Title,
    string Description,
    DateTime Deadline,
    bool IsStarted) : IRequest<ReadModel>;