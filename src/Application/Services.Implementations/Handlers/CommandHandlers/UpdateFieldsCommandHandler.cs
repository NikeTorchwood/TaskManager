using AutoMapper;
using Domain.Entities;
using Domain.ValueObjects;
using MediatR;
using Repositories.Abstractions;
using Services.Contracts.Commands;
using Services.Contracts.Models;
using static Common.Resources.Constants.DescriptionConstants;
using static Common.Resources.Constants.TitleConstants;
using static Common.Resources.ResponseErrorMessages.ErrorMessages;

namespace Services.Implementations.Handlers.CommandHandlers;

/// <summary>
/// Handler for processing the <see cref="UpdateFieldsCommand"/> to update task point fields.
/// </summary>
internal class UpdateFieldsCommandHandler(
    IReadTaskPointsRepository readRepository,
    IWriteTaskPointsRepository writeRepository,
    IMapper mapper)
    : IRequestHandler<UpdateFieldsCommand, ResultModel<ReadModel>>
{
    /// <summary>
    /// Handles the <see cref="UpdateFieldsCommand"/> to update the fields of a task point.
    /// </summary>
    /// <param name="request">The command request containing the task point ID and the fields to update.</param>
    /// <param name="ct">A cancellation token for the operation.</param>
    /// <returns>A result model containing the updated task point or an error message if the update is not possible.</returns>
    public async Task<ResultModel<ReadModel>> Handle(
        UpdateFieldsCommand request,
        CancellationToken ct)
    {
        var taskPoint = await readRepository.GetByIdAsync(request.Id, ct);
        if (taskPoint is null)
            return FailureResult(ERROR_MESSAGE_TASK_NOT_FOUND);

        var validationResult = ValidateRequest(taskPoint, request);
        if (validationResult is not null)
            return validationResult;

        UpdateTaskPointFields(taskPoint, request);

        await writeRepository.UpdateAsync(taskPoint, ct);

        var result = mapper.Map<ReadModel>(taskPoint);

        return ResultModel<ReadModel>.SuccessResult(result);
    }

    /// <summary>
    /// Validates the fields in the update command.
    /// </summary>
    /// <param name="taskPoint">The task point entity to validate.</param>
    /// <param name="request">The update command containing the new values.</param>
    /// <returns>A <see cref="ResultModel{ReadModel}"/> with an error message if validation fails, otherwise null.</returns>
    private static ResultModel<ReadModel>? ValidateRequest(TaskPoint taskPoint, UpdateFieldsCommand request)
    {
        if (taskPoint.ClosedAt.HasValue && taskPoint.ClosedAt.Value < DateTime.UtcNow)
            return FailureResult(ERROR_MESSAGE_TASK_ALREADY_CLOSED);

        if (taskPoint.IsDeleted)
            return FailureResult(ERROR_MESSAGE_TASK_WAS_DELETED);

        if (!string.IsNullOrWhiteSpace(request.NewTitle))
        {
            switch (request.NewTitle.Length)
            {
                case < TITLE_MIN_LENGTH:
                    return FailureResult(ERROR_MESSAGE_TITLE_SHORTER_MIN_LENGTH);
                case > TITLE_MAX_LENGTH:
                    return FailureResult(ERROR_MESSAGE_TITLE_LONGER_MAX_LENGTH);
            }
        }

        if (!string.IsNullOrWhiteSpace(request.NewDescription))
        {
            switch (request.NewDescription.Length)
            {
                case < DESCRIPTION_MIN_LENGTH:
                    return FailureResult(ERROR_MESSAGE_DESCRIPTION_SHORTER_MIN_LENGTH);
                case > DESCRIPTION_MAX_LENGTH:
                    return FailureResult(ERROR_MESSAGE_DESCRIPTION_LONGER_MAX_LENGTH);
            }
        }

        if (request.NewDeadline.HasValue && request.NewDeadline.Value <= DateTime.UtcNow)
            return FailureResult(ERROR_MESSAGE_DEADLINE_MUST_BE_IN_FUTURE);

        return null;
    }

    /// <summary>
    /// Updates the fields of the task point with the values from the request.
    /// </summary>
    /// <param name="taskPoint">The task point entity to update.</param>
    /// <param name="request">The update command containing the new values.</param>
    private static void UpdateTaskPointFields(TaskPoint taskPoint, UpdateFieldsCommand request)
    {
        if (!string.IsNullOrWhiteSpace(request.NewTitle) && request.NewTitle != taskPoint.Title.Value)
        {
            var newTitle = new Title(request.NewTitle);
            taskPoint.ChangeTitle(newTitle);
        }

        if (!string.IsNullOrWhiteSpace(request.NewDescription) && request.NewDescription != taskPoint.Description.Value)
        {
            var newDescription = new Description(request.NewDescription);
            taskPoint.ChangeDescription(newDescription);
        }

        if (request.NewDeadline.HasValue && request.NewDeadline.Value != taskPoint.Deadline)
        {
            taskPoint.ChangeDeadline(request.NewDeadline.Value);
        }
    }

    /// <summary>
    /// Creates a failure result with the given error message.
    /// </summary>
    /// <param name="errorMessage">The error message to include in the result.</param>
    /// <returns>A failure result with the provided error message.</returns>
    private static ResultModel<ReadModel> FailureResult(string errorMessage)
    {
        return ResultModel<ReadModel>.FailureResult(errorMessage);
    }
}