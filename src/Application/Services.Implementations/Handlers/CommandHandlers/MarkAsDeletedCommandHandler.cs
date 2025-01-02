using MediatR;
using Repositories.Abstractions;
using Services.Contracts.Commands;
using Services.Contracts.Models;
using static Common.Resources.ResponseErrorMessages.ErrorMessages;

namespace Services.Implementations.Handlers.CommandHandlers;

internal class MarkAsDeletedCommandHandler(
    IWriteTaskPointsRepository writeRepository,
    IReadTaskPointsRepository readRepository)
    : IRequestHandler<MarkAsDeletedCommand, ResultModel<bool>>
{
    public async Task<ResultModel<bool>> Handle(
        MarkAsDeletedCommand request,
        CancellationToken ct)
    {
        var taskPoint = await readRepository.GetByIdAsync(request.Id, ct);
        if (taskPoint is null)
            return ResultModel<bool>.FailureResult(ERROR_MESSAGE_TASK_NOT_FOUND);

        taskPoint.MarkAsDeleted();

        await writeRepository.UpdateAsync(taskPoint, ct);

        return ResultModel<bool>.SuccessResult(true);
    }
}