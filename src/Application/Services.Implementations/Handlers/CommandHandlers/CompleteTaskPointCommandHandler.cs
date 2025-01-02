using MediatR;
using Repositories.Abstractions;
using Services.Contracts.Commands;
using Services.Contracts.Models;
using static Common.Resources.ResponseErrorMessages.ErrorMessages;

namespace Services.Implementations.Handlers.CommandHandlers;

internal class CompleteTaskPointCommandHandler(
    IWriteTaskPointsRepository writeRepository,
    IReadTaskPointsRepository readRepository)
    : IRequestHandler<CompleteTaskPointCommand, ResultModel<bool>>
{
    public async Task<ResultModel<bool>> Handle(
        CompleteTaskPointCommand request,
        CancellationToken ct)
    {
        var taskPoint = await readRepository.GetByIdAsync(request.Id, ct);
        if (taskPoint is null)
            return ResultModel<bool>.FailureResult(ERROR_MESSAGE_TASK_NOT_FOUND);

        if (taskPoint.ClosedAt.HasValue)
            return ResultModel<bool>.FailureResult(ERROR_MESSAGE_CANT_COMPLETE_CLOSED_TASK);
        if (!taskPoint.StartedAt.HasValue)
            return ResultModel<bool>.FailureResult(ERROR_MESSAGE_CANT_COMPLETE_NOT_OPENED_TASK);

        taskPoint.CompleteTask();

        await writeRepository.UpdateAsync(taskPoint, ct);

        return ResultModel<bool>.SuccessResult(true);
    }
}