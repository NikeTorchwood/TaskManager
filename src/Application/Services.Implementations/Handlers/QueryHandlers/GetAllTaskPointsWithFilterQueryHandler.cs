using Domain.Entities;
using MediatR;
using QueryFilterBuilder;
using Repositories.Abstractions;
using Services.Contracts.Models;
using Services.Contracts.Queries;

namespace Services.Implementations.Handlers.QueryHandlers;

internal class GetAllTaskPointsWithFilterQueryHandler(
    IReadTaskPointsRepository taskPointsRepository,
    IMapper mapper)
    : IRequestHandler<GetAllTaskPointsWithFilterQuery, IEnumerable<ReadModel>>
{
    public async Task<IEnumerable<ReadModel>> Handle(
        GetAllTaskPointsWithFilterQuery request,
        CancellationToken ct)
    {
        var query = await taskPointsRepository.GetAllAsync(ct);
        var queryBuilder = new QueryFilterBuilder<TaskPoint>();

        foreach (var filter in request.Filters)
        {
            queryBuilder.AddFilter(filter.Apply());
        }
        var predicate = queryBuilder.Build();

        var filteredTaskPoints = query.Where(predicate).ToList();

        return mapper.Map<IEnumerable<ReadModel>>(filteredTaskPoints);
    }
}