using MediatR;
using Services.Contracts.Models;

namespace Services.Contracts.Commands;

public record CompleteTaskPointCommand(Guid Id) : IRequest<ResultModel<bool>>;