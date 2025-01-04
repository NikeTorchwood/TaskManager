using MediatR;
using Repositories.Abstractions;
using Services.Contracts.Commands;
using Services.Contracts.Models;
using static Common.Resources.ResponseErrorMessages.ErrorMessages;

namespace Services.Implementations.Handlers.CommandHandlers;

/// <summary>
/// Handler for processing the <see cref="StartTaskPointCommand"/> to start a task point.
/// </summary>
public class StartTaskPointCommandHandler(
    IWriteTaskPointsRepository writeRepository,
    IReadTaskPointsRepository readRepository)
    : IRequestHandler<StartTaskPointCommand, ResultModel<bool>>
{
    /// <summary>
    /// Handles the <see cref="StartTaskPointCommand"/> to start a task point.
    /// </summary>
    /// <param name="request">The command request containing the task point ID to be started.</param>
    /// <param name="ct">A cancellation token for the operation.</param>
    /// <returns>A result model indicating whether the task point was successfully started or not.</returns>
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
