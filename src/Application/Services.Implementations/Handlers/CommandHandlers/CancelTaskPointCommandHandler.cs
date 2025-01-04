using MediatR;
using Repositories.Abstractions;
using Services.Contracts.Commands;
using Services.Contracts.Models;
using static Common.Resources.ResponseErrorMessages.ErrorMessages;

namespace Services.Implementations.Handlers.CommandHandlers;

/// <summary>
/// Handler for processing the <see cref="CancelTaskPointCommand"/>.
/// </summary>
internal class CancelTaskPointCommandHandler(
    IWriteTaskPointsRepository writeRepository,
    IReadTaskPointsRepository readRepository)
    : IRequestHandler<CancelTaskPointCommand, ResultModel<bool>>
{
    /// <summary>
    /// Handles the <see cref="CancelTaskPointCommand"/> to cancel a task point.
    /// </summary>
    /// <param name="request">The command request containing the task point ID to cancel.</param>
    /// <param name="ct">A cancellation token for the operation.</param>
    /// <returns>A result model indicating whether the cancellation was successful or not.</returns>
    public async Task<ResultModel<bool>> Handle(
        CancelTaskPointCommand request,
        CancellationToken ct)
    {
        var taskPoint = await readRepository.GetByIdAsync(request.Id, ct);
        if (taskPoint is null)
            return ResultModel<bool>.FailureResult(ERROR_MESSAGE_TASK_NOT_FOUND);

        if (taskPoint.ClosedAt.HasValue)
            return ResultModel<bool>.FailureResult(ERROR_MESSAGE_TASK_ALREADY_CLOSED);

        taskPoint.CancelTask();

        await writeRepository.UpdateAsync(taskPoint, ct);

        return ResultModel<bool>.SuccessResult(true);
    }
}