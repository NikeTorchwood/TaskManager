using AutoMapper;
using Domain.Entities;
using Domain.ValueObjects;
using MediatR;
using Repositories.Abstractions;
using Services.Contracts.Commands;
using Services.Contracts.Models;
using static Common.Resources.ResponseErrorMessages.ErrorMessages;

namespace Services.Implementations.Handlers.CommandHandlers;

internal class CreateTaskPointCommandHandler(
    IWriteTaskPointsRepository repository,
    IMapper mapper)
    : IRequestHandler<CreateTaskPointCommand, ResultModel<ReadModel>>
{
    public async Task<ResultModel<ReadModel>> Handle(
        CreateTaskPointCommand request,
        CancellationToken ct)
    {
        if (request == null ||
            string.IsNullOrWhiteSpace(request.Title) ||
            string.IsNullOrWhiteSpace(request.Description) ||
            request.Deadline <= DateTime.UtcNow)
            return ResultModel<ReadModel>.FailureResult(ERROR_MESSAGE_INVALID_DATA);

        var title = new Title(request.Title);
        var description = new Description(request.Description);

        var taskPoint = new TaskPoint(title, description, request.Deadline, request.IsStarted);

        var createdTaskPoint = await repository.AddAsync(taskPoint, ct);

        var result = mapper.Map<ReadModel>(createdTaskPoint);

        return ResultModel<ReadModel>.SuccessResult(result);
    }
}