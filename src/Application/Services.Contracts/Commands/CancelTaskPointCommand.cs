using MediatR;
using Services.Contracts.Models;

namespace Services.Contracts.Commands;

public record CancelTaskPointCommand(Guid Id) : IRequest<ResultModel<bool>>;