using Domain.Entities;
using MediatR;
using Services.Contracts.Filters;
using Services.Contracts.Models;

namespace Services.Contracts.Queries;

public record GetAllTaskPointsWithFilterQuery(IEnumerable<IFilter<TaskPoint>> Filters) : IRequest<IEnumerable<ReadModel>>;