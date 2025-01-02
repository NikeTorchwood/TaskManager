using AutoMapper;
using Domain.ValueObjects;
using MediatR;
using Repositories.Abstractions;
using Services.Contracts.Commands;
using Services.Contracts.Models;
using static Common.Resources.Constants.DescriptionConstants;
using static Common.Resources.Constants.TitleConstants;
using static Common.Resources.ResponseErrorMessages.ErrorMessages;

namespace Services.Implementations.Handlers.CommandHandlers;

internal class UpdateFieldsCommandHandler(
    IReadTaskPointsRepository readRepository,
    IWriteTaskPointsRepository writeRepository,
    IMapper mapper)
    : IRequestHandler<UpdateFieldsCommand,
        ResultModel<ReadModel>>
{
    public async Task<ResultModel<ReadModel>> Handle(
        UpdateFieldsCommand request,
        CancellationToken ct)
    {
        var taskPoint = await readRepository.GetByIdAsync(request.Id, ct);
        if (taskPoint is null)
            return ResultModel<ReadModel>.FailureResult(ERROR_MESSAGE_TASK_NOT_FOUND);

        if (taskPoint.ClosedAt.HasValue && taskPoint.ClosedAt.Value < DateTime.UtcNow)
            return ResultModel<ReadModel>.FailureResult(ERROR_MESSAGE_TASK_ALREADY_CLOSED);
        if (!string.IsNullOrWhiteSpace(request.NewTitle) 
            && request.NewTitle.Length < TITLE_MIN_LENGTH)
            return ResultModel<ReadModel>.FailureResult(ERROR_MESSAGE_TITLE_SHORTER_MIN_LENGTH);
        if (!string.IsNullOrWhiteSpace(request.NewTitle) 
            && request.NewTitle.Length > TITLE_MAX_LENGTH)
            return ResultModel<ReadModel>.FailureResult(ERROR_MESSAGE_TITLE_LONGER_MAX_LENGTH);

        if (!string.IsNullOrWhiteSpace(request.NewDescription) 
            && request.NewDescription.Length < DESCRIPTION_MIN_LENGTH)
            return ResultModel<ReadModel>.FailureResult(ERROR_MESSAGE_DESCRIPTION_SHORTER_MIN_LENGTH);
        if (!string.IsNullOrWhiteSpace(request.NewDescription) 
            && request.NewDescription.Length > DESCRIPTION_MAX_LENGTH)
            return ResultModel<ReadModel>.FailureResult(ERROR_MESSAGE_DESCRIPTION_LONGER_MAX_LENGTH);

        if (request.NewDeadline.HasValue && request.NewDeadline.Value <= DateTime.UtcNow)
            return ResultModel<ReadModel>.FailureResult(ERROR_MESSAGE_DEADLINE_MUST_BE_IN_FUTURE);

        if (!string.IsNullOrWhiteSpace(request.NewTitle) 
            && request.NewTitle != taskPoint.Title.Value)
        {
            var newTitle = new Title(request.NewTitle);
            taskPoint.ChangeTitle(newTitle);
        }

        if (!string.IsNullOrWhiteSpace(request.NewDescription)
            && request.NewDescription != taskPoint.Description.Value)
        {
            var description = new Description(request.NewDescription);
            taskPoint.ChangeDescription(description);
        }

        if (request.NewDeadline.HasValue
            && request.NewDeadline.Value != taskPoint.Deadline)
            taskPoint.ChangeDeadline(request.NewDeadline.Value);

        await writeRepository.UpdateAsync(taskPoint, ct);
        var result = mapper.Map<ReadModel>(taskPoint);
        return ResultModel<ReadModel>.SuccessResult(result);
    }
}