using MediatR;
using Repositories.Abstractions;
using Services.Contracts.Commands;
using Services.Contracts.Models;
using static Common.Resources.ResponseErrorMessages.ErrorMessages;

namespace Services.Implementations.Handlers.CommandHandlers;

internal class StartTaskPointCommandHandler(
    IWriteTaskPointsRepository writeRepository,
    IReadTaskPointsRepository readRepository)
    : IRequestHandler<StartTaskPointCommand, ResultModel<bool>>
{
    public async Task<ResultModel<bool>> Handle(
        StartTaskPointCommand request,
        CancellationToken ct)
    {
        var taskPoint = await readRepository.GetByIdAsync(request.Id, ct);
        if (taskPoint is null)
            return ResultModel<bool>.FailureResult(ERROR_MESSAGE_TASK_NOT_FOUND);

        if (taskPoint.ClosedAt.HasValue)
            return ResultModel<bool>.FailureResult(ERROR_MESSAGE_CANT_START_CLOSED_TASK);
        if (taskPoint.StartedAt.HasValue)
            return ResultModel<bool>.FailureResult(ERROR_MESSAGE_TASK_ALREADY_STARTED);

        taskPoint.StartTask();

        await writeRepository.UpdateAsync(taskPoint, ct);

        return ResultModel<bool>.SuccessResult(true);
    }
}