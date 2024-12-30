using MediatR;
using Repositories.Abstractions;
using Services.Abstractions;
using Services.Contracts.Commands;
using Services.Contracts.Models;
using Services.Contracts.Queries;

namespace Services.Implementations;

public class TaskPointsService(
    IMediator mediator,
    IWriteTaskPointsRepository repository) : ITaskPointService
{

    public async Task<ResultModel<ReadModel>> GetTaskPointByIdAsync(
        Guid id,
        CancellationToken ct)
    {
        var query = new GetTaskPointByIdQuery(id);
        var result = await mediator.Send(query, ct);
        return result is null
            ? ResultModel<ReadModel>.FailureResult("Task not found.")
            : ResultModel<ReadModel>.SuccessResult(result);
    }

    public async Task<IEnumerable<ReadModel>> GetAllTaskPointsWithFilterAsync(
        FilterModel filter,
        CancellationToken ct)
    {
        var query = new GetAllTaskPointsWithFilterQuery(filter.Filters);
        return await mediator.Send(query, ct);
    }

    public async Task<ResultModel<ReadModel>> CreateTaskPointAsync(
        CreateTaskPointCommand command,
        CancellationToken ct)
    {
        if (command == null ||
            string.IsNullOrWhiteSpace(command.Title) ||
            string.IsNullOrWhiteSpace(command.Description) ||
            command.Deadline <= DateTime.UtcNow)
            return ResultModel<ReadModel>.FailureResult("Invalid task data.");

        var result = await mediator.Send(command, ct);

        return ResultModel<ReadModel>.SuccessResult(result);
    }

    //public Task ChangeTaskPointTitleAsync(ChangeTitleModel model, CancellationToken ct)
    //{
    //    throw new NotImplementedException();
    //}

    //public Task ChangeTaskPointDescriptionAsync(ChangeDescriptionModel model, CancellationToken ct)
    //{
    //    throw new NotImplementedException();
    //}

    //public Task ChangeTaskPointDeadlineAsync(ChangeDeadlineModel model, CancellationToken ct)
    //{
    //    throw new NotImplementedException();
    //}

    //public Task StartTaskPointAsync(Guid id, CancellationToken ct)
    //{
    //    throw new NotImplementedException();
    //}

    //public Task CompleteTaskPointAsync(Guid id, CancellationToken ct)
    //{
    //    throw new NotImplementedException();
    //}

    //public Task CancelTaskPointAsync(Guid id, CancellationToken ct)
    //{
    //    throw new NotImplementedException();
    //}

    //public Task MarkAsDeletedTaskPointAsync(Guid id, CancellationToken ct)
    //{
    //    throw new NotImplementedException();
    //}
}