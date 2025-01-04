using MediatR;
using Repositories.Abstractions;
using Services.Contracts.Commands;
using Services.Contracts.Models;
using static Common.Resources.ResponseErrorMessages.ErrorMessages;

namespace Services.Implementations.Handlers.CommandHandlers;

/// <summary>
/// Handler for processing the <see cref="CompleteTaskPointCommand"/>.
/// </summary>
public class CompleteTaskPointCommandHandler(
    IWriteTaskPointsRepository writeRepository,
    IReadTaskPointsRepository readRepository)
    : IRequestHandler<CompleteTaskPointCommand, ResultModel<bool>>
{
    /// <summary>
    /// Handles the <see cref="CompleteTaskPointCommand"/> to complete a task point.
    /// </summary>
    /// <param name="request">The command request containing the task point ID to complete.</param>
    /// <param name="ct">A cancellation token for the operation.</param>
    /// <returns>A result model indicating whether the completion was successful or not.</returns>
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