using AutoMapper;
using MediatR;
using Repositories.Abstractions;
using Services.Contracts.Models;
using Services.Contracts.Queries;

namespace Services.Implementations.Handlers.QueryHandlers;

/// <summary>
/// Handles the query to get all task points with filters applied.
/// </summary>
/// <remarks>
/// This handler fetches all task points from the repository and applies the provided filter
/// to return the filtered result set as a list of <see cref="ReadModel"/>.
/// </remarks>
internal class GetAllTaskPointsWithFilterQueryHandler(
    IReadTaskPointsRepository taskPointsRepository,
    IMapper mapper)
    : IRequestHandler<GetAllTaskPointsWithFilterQuery, IEnumerable<ReadModel>>
{
    /// <summary>
    /// Handles the request to get all task points with filters.
    /// </summary>
    /// <param name="request">The query containing the filter expression to apply on the task points.</param>
    /// <param name="ct">The cancellation token to observe while waiting for the operation to complete.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the filtered list of <see cref="ReadModel"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the <paramref name="request"/> is null.</exception>
    public async Task<IEnumerable<ReadModel>> Handle(
        GetAllTaskPointsWithFilterQuery request,
        CancellationToken ct)
    {
        var query = await taskPointsRepository.GetAllAsync(ct);

        var filteredTaskPoints = query.Where(request.Filters).ToList();

        return mapper.Map<IEnumerable<ReadModel>>(filteredTaskPoints);
    }
}
