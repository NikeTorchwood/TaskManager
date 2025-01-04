using Services.Contracts.Commands;
using Services.Contracts.Models;

namespace Services.Abstractions;

/// <summary>
/// Defines the service interface for managing task points.
/// </summary>
public interface ITaskPointsService
{
    /// <summary>
    /// Asynchronously retrieves a task point by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the task point.</param>
    /// <param name="ct">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A result model containing the task point details if found.</returns>
    public Task<ResultModel<ReadModel>> GetTaskPointByIdAsync(Guid id, CancellationToken ct);

    /// <summary>
    /// Asynchronously retrieves all task points that match the provided filter criteria.
    /// </summary>
    /// <param name="model">The filter model containing the filtering criteria.</param>
    /// <param name="ct">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>An enumerable collection of task point models.</returns>
    public Task<IEnumerable<ReadModel>> GetAllTaskPointsWithFilterAsync(FilterModel model, CancellationToken ct);

    /// <summary>
    /// Asynchronously creates a new task point.
    /// </summary>
    /// <param name="command">The command containing the details for creating the task point.</param>
    /// <param name="ct">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A result model containing the created task point.</returns>
    public Task<ResultModel<ReadModel>> CreateTaskPointAsync(CreateTaskPointCommand command, CancellationToken ct);

    /// <summary>
    /// Asynchronously starts a task point by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the task point to start.</param>
    /// <param name="ct">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A result model indicating whether the operation was successful.</returns>
    public Task<ResultModel<bool>> StartTaskPointAsync(Guid id, CancellationToken ct);

    /// <summary>
    /// Asynchronously marks a task point as complete by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the task point to complete.</param>
    /// <param name="ct">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A result model indicating whether the operation was successful.</returns>
    public Task<ResultModel<bool>> CompleteTaskPointAsync(Guid id, CancellationToken ct);

    /// <summary>
    /// Asynchronously cancels a task point by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the task point to cancel.</param>
    /// <param name="ct">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A result model indicating whether the operation was successful.</returns>
    public Task<ResultModel<bool>> CancelTaskPointAsync(Guid id, CancellationToken ct);

    /// <summary>
    /// Asynchronously marks a task point as deleted by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the task point to mark as deleted.</param>
    /// <param name="ct">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A result model indicating whether the operation was successful.</returns>
    public Task<ResultModel<bool>> MarkAsDeletedTaskPointAsync(Guid id, CancellationToken ct);

    /// <summary>
    /// Asynchronously updates specific fields of a task point based on the provided command.
    /// </summary>
    /// <param name="command">The command containing the fields to update.</param>
    /// <param name="request">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A result model containing the updated task point details.</returns>
    public Task<ResultModel<ReadModel>> UpdateTaskPointFieldsAsync(UpdateFieldsCommand command, CancellationToken request);
}
