using Domain.Entities;
using MediatR;
using Services.Contracts.Models;

namespace Services.Contracts.Queries;

/// <summary>
/// Query for retrieving a <see cref="TaskPoint"/> entity by its unique identifier.
/// </summary>
/// <param name="Id">The unique identifier of the <see cref="TaskPoint"/> entity to be retrieved.</param>
public record GetTaskPointByIdQuery(Guid Id)
    : IRequest<ResultModel<ReadModel>>;