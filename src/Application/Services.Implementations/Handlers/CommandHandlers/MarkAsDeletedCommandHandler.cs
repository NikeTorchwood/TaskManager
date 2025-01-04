using MediatR;
using Repositories.Abstractions;
using Services.Contracts.Commands;
using Services.Contracts.Models;
using static Common.Resources.ResponseErrorMessages.ErrorMessages;

namespace Services.Implementations.Handlers.CommandHandlers;

/// <summary>
/// Handler for processing the <see cref="MarkAsDeletedCommand"/> to mark a task point as deleted.
/// </summary>
internal class MarkAsDeletedCommandHandler(
    IWriteTaskPointsRepository writeRepository,
    IReadTaskPointsRepository readRepository)
    : IRequestHandler<MarkAsDeletedCommand, ResultModel<bool>>
{
    /// <summary>
    /// Handles the <see cref="MarkAsDeletedCommand"/> to mark a task point as deleted.
    /// </summary>
    /// <param name="request">The command request containing the task point ID to be marked as deleted.</param>
    /// <param name="ct">A cancellation token for the operation.</param>
    /// <returns>A result model indicating whether the task point was successfully marked as deleted.</returns>
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