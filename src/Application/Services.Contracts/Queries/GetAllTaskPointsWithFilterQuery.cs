using Domain.Entities;
using MediatR;
using Services.Contracts.Models;
using System.Linq.Expressions;

namespace Services.Contracts.Queries;

/// <summary>
/// Query for retrieving all <see cref="TaskPoint"/> entities that match the specified filters.
/// </summary>
/// <param name="Filters">
/// An expression defining the filtering logic to be applied to <see cref="TaskPoint"/> entities.
/// </param>
public record GetAllTaskPointsWithFilterQuery(
    Expression<Func<TaskPoint, bool>> Filters)
    : IRequest<IEnumerable<ReadModel>>;