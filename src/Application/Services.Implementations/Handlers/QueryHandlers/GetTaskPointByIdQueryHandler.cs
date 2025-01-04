using AutoMapper;
using MediatR;
using Repositories.Abstractions;
using Services.Contracts.Models;
using Services.Contracts.Queries;
using static Common.Resources.ResponseErrorMessages.ErrorMessages;

namespace Services.Implementations.Handlers.QueryHandlers;

/// <summary>
/// Handles the query to retrieve a task point by its ID.
/// </summary>
/// <remarks>
/// This handler fetches a task point from the repository by its ID. If the task point
/// is found, it maps it to the <see cref="ReadModel"/>; otherwise, it returns a failure result.
/// </remarks>
public class GetTaskPointByIdQueryHandler(
    IReadTaskPointsRepository repository,
    IMapper mapper)
    : IRequestHandler<GetTaskPointByIdQuery, ResultModel<ReadModel>>
{
    /// <summary>
    /// Handles the request to retrieve a task point by its ID.
    /// </summary>
    /// <param name="request">The query containing the task point ID.</param>
    /// <param name="ct">The cancellation token to observe while waiting for the operation to complete.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="ResultModel{ReadModel}"/> with the task point data if found, otherwise a failure result.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the <paramref name="request"/> is null.</exception>
    public async Task<ResultModel<ReadModel>> Handle(
        GetTaskPointByIdQuery request,
        CancellationToken ct)
    {
        var taskPoint = await repository.GetByIdAsync(request.Id, ct);
        if (taskPoint is null)
            return ResultModel<ReadModel>.FailureResult(ERROR_MESSAGE_TASK_NOT_FOUND);

        var model = mapper.Map<ReadModel>(taskPoint);

        return ResultModel<ReadModel>.SuccessResult(model);
    }
}
