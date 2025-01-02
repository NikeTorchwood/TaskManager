using Domain.Entities;
using MediatR;
using Services.Contracts.Models;
using System.Linq.Expressions;

namespace Services.Contracts.Queries;

public record GetAllTaskPointsWithFilterQuery(Expression<Func<TaskPoint, bool>> Filters) : IRequest<IEnumerable<ReadModel>>;