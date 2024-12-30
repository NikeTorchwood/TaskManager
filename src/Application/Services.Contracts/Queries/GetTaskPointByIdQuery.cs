using MediatR;
using Services.Contracts.Models;

namespace Services.Contracts.Queries;

public record GetTaskPointByIdQuery(Guid Id) : IRequest<ReadModel>;