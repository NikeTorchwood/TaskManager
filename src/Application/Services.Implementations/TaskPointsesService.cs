using Domain.Entities;
using MediatR;
using QueryFilterBuilder;
using Services.Abstractions;
using Services.Contracts.Commands;
using Services.Contracts.Models;
using Services.Contracts.Queries;

namespace Services.Implementations;

/// <summary>
/// Service for managing task points, providing methods to retrieve, create, update, and perform actions on task points.
/// </summary>
/// <remarks>
/// This service acts as a bridge between the application's business logic and the Mediator pattern, delegating tasks
/// to the appropriate handlers via the <see cref="IMediator"/> interface. It provides high-level methods for interacting
/// with task points, including filtering and applying various actions like creating, starting, completing, and deleting tasks.
/// </remarks>
public class TaskPointsService(
    IMediator mediator) : ITaskPointsService
{
    /// <summary>
    /// Retrieves a task point by its ID.
    /// </summary>
    /// <param name="id">The ID of the task point to retrieve.</param>
    /// <param name="ct">The cancellation token to observe while waiting for the operation to complete.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the <see cref="ResultModel{ReadModel}"/> with task point data if found, otherwise a failure result.</returns>
    public async Task<ResultModel<ReadModel>> GetTaskPointByIdAsync(
        Guid id,
        CancellationToken ct)
    {
        var query = new GetTaskPointByIdQuery(id);
        return await mediator.Send(query, ct);
    }

    /// <summary>
    /// Retrieves all task points with applied filters.
    /// </summary>
    /// <param name="model">The model containing filters to apply on the task points.</param>
    /// <param name="ct">The cancellation token to observe while waiting for the operation to complete.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains an <see cref="IEnumerable{ReadModel}"/> of filtered task points.</returns>
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

    /// <summary>
    /// Creates a new task point.
    /// </summary>
    /// <param name="command">The command containing the data to create the task point.</param>
    /// <param name="ct">The cancellation token to observe while waiting for the operation to complete.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the created <see cref="ReadModel"/> if successful.</returns>
    public async Task<ResultModel<ReadModel>> CreateTaskPointAsync(
        CreateTaskPointCommand command,
        CancellationToken ct)
        => await mediator.Send(command, ct);

    /// <summary>
    /// Starts a task point by its ID.
    /// </summary>
    /// <param name="id">The ID of the task point to start.</param>
    /// <param name="ct">The cancellation token to observe while waiting for the operation to complete.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains a boolean indicating success.</returns>
    public async Task<ResultModel<bool>> StartTaskPointAsync(Guid id, CancellationToken ct)
        => await mediator.Send(new StartTaskPointCommand(id), ct);

    /// <summary>
    /// Completes a task point by its ID.
    /// </summary>
    /// <param name="id">The ID of the task point to complete.</param>
    /// <param name="ct">The cancellation token to observe while waiting for the operation to complete.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains a boolean indicating success.</returns>
    public async Task<ResultModel<bool>> CompleteTaskPointAsync(Guid id, CancellationToken ct)
        => await mediator.Send(new CompleteTaskPointCommand(id), ct);

    /// <summary>
    /// Cancels a task point by its ID.
    /// </summary>
    /// <param name="id">The ID of the task point to cancel.</param>
    /// <param name="ct">The cancellation token to observe while waiting for the operation to complete.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains a boolean indicating success.</returns>
    public async Task<ResultModel<bool>> CancelTaskPointAsync(Guid id, CancellationToken ct)
        => await mediator.Send(new CancelTaskPointCommand(id), ct);

    /// <summary>
    /// Marks a task point as deleted by its ID.
    /// </summary>
    /// <param name="id">The ID of the task point to mark as deleted.</param>
    /// <param name="ct">The cancellation token to observe while waiting for the operation to complete.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains a boolean indicating success.</returns>
    public async Task<ResultModel<bool>> MarkAsDeletedTaskPointAsync(Guid id, CancellationToken ct)
        => await mediator.Send(new MarkAsDeletedCommand(id), ct);

    /// <summary>
    /// Updates the fields of a task point.
    /// </summary>
    /// <param name="command">The command containing the new field values for the task point.</param>
    /// <param name="ct">The cancellation token to observe while waiting for the operation to complete.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the updated <see cref="ReadModel"/> if successful.</returns>
    public async Task<ResultModel<ReadModel>> UpdateTaskPointFieldsAsync(UpdateFieldsCommand command, CancellationToken ct)
        => await mediator.Send(command, ct);
}