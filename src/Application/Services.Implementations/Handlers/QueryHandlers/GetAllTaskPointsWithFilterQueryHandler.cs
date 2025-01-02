using AutoMapper;
using MediatR;
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

        var filteredTaskPoints = query.Where(request.Filters).ToList();

        return mapper.Map<IEnumerable<ReadModel>>(filteredTaskPoints);
    }
}