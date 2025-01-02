using MediatR;
using Services.Contracts.Models;

namespace Services.Contracts.Commands;

public record StartTaskPointCommand(Guid Id) : IRequest<ResultModel<bool>>;