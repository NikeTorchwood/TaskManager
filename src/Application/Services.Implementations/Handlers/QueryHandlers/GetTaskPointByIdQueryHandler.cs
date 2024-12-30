using MediatR;
using Repositories.Abstractions;
using Services.Contracts.Models;
using Services.Contracts.Queries;

namespace Services.Implementations.Handlers.QueryHandlers;

internal class GetTaskPointByIdQueryHandler(
    IReadTaskPointsRepository repository,
    IMapper mapper)
    : IRequestHandler<GetTaskPointByIdQuery, ReadModel>
{
    public async Task<ReadModel> Handle(
        GetTaskPointByIdQuery request,
        CancellationToken ct)
    {
        var result = await repository.GetByIdAsync(request.Id, ct);
        return mapper.Map<ReadModel>(result);
    }
}