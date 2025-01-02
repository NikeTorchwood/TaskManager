using AutoMapper;
using MediatR;
using Repositories.Abstractions;
using Services.Contracts.Models;
using Services.Contracts.Queries;

namespace Services.Implementations.Handlers.QueryHandlers;

internal class GetTaskPointByIdQueryHandler(
    IReadTaskPointsRepository repository,
    IMapper mapper)
    : IRequestHandler<GetTaskPointByIdQuery, ResultModel<ReadModel>>
{
    public async Task<ResultModel<ReadModel>> Handle(
        GetTaskPointByIdQuery request,
        CancellationToken ct)
    {
        var taskPoint = await repository.GetByIdAsync(request.Id, ct);
        if (taskPoint is null)
            return ResultModel<ReadModel>.FailureResult("Task not found.");
        var model = mapper.Map<ReadModel>(taskPoint);

        return ResultModel<ReadModel>.SuccessResult(model);
    }
}