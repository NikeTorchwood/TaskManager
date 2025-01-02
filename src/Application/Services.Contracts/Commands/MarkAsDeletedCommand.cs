using MediatR;
using Services.Contracts.Models;

namespace Services.Contracts.Commands;

public record MarkAsDeletedCommand(Guid Id) : IRequest<ResultModel<bool>>;