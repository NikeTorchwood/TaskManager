using Domain.Entities;
using MediatR;
using QueryFilterBuilder;
using Services.Abstractions;
using Services.Contracts.Commands;
using Services.Contracts.Models;
using Services.Contracts.Queries;

namespace Services.Implementations;

public class TaskPointsService(
    IMediator mediator) : ITaskPointsService
{

    public async Task<ResultModel<ReadModel>> GetTaskPointByIdAsync(
        Guid id,
        CancellationToken ct)
    {
        var query = new GetTaskPointByIdQuery(id);
        return await mediator.Send(query, ct);
    }

    public async Task<IEnumerable<ReadModel>> GetAllTaskPointsWithFilterAsync(
        FilterModel model,
        CancellationToken ct)
    {
        var queryBuilder = new QueryFilterBuilder<TaskPoint>();

        foreach (var filter in model.Filters)
        {
            queryBuilder.AddFilter(filter.Apply());
        }
        var predicate = queryBuilder.Build();

        var query = new GetAllTaskPointsWithFilterQuery(predicate);

        return await mediator.Send(query, ct);
    }

    public async Task<ResultModel<ReadModel>> CreateTaskPointAsync(
        CreateTaskPointCommand command,
        CancellationToken ct)
        => await mediator.Send(command, ct);

    public async Task<ResultModel<bool>> StartTaskPointAsync(Guid id, CancellationToken ct)
        => await mediator.Send(new StartTaskPointCommand(id), ct);

    public async Task<ResultModel<bool>> CompleteTaskPointAsync(Guid id, CancellationToken ct)
        => await mediator.Send(new CompleteTaskPointCommand(id), ct);

    public async Task<ResultModel<bool>> CancelTaskPointAsync(Guid id, CancellationToken ct)
        => await mediator.Send(new CancelTaskPointCommand(id), ct);

    public async Task<ResultModel<bool>> MarkAsDeletedTaskPointAsync(Guid id, CancellationToken ct)
        => await mediator.Send(new MarkAsDeletedCommand(id), ct);

    public async Task<ResultModel<ReadModel>> UpdateTaskPointFieldsAsync(UpdateFieldsCommand command, CancellationToken ct)
        => await mediator.Send(command, ct);
}