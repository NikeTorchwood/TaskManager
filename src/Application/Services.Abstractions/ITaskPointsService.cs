using Services.Contracts.Commands;
using Services.Contracts.Models;

namespace Services.Abstractions;

public interface ITaskPointsService
{
    public Task<ResultModel<ReadModel>> GetTaskPointByIdAsync(Guid id, CancellationToken ct);

    public Task<IEnumerable<ReadModel>> GetAllTaskPointsWithFilterAsync(FilterModel model, CancellationToken ct);

    public Task<ResultModel<ReadModel>> CreateTaskPointAsync(CreateTaskPointCommand command, CancellationToken ct);

    public Task<ResultModel<bool>> StartTaskPointAsync(Guid id, CancellationToken ct);

    public Task<ResultModel<bool>> CompleteTaskPointAsync(Guid id, CancellationToken ct);

    public Task<ResultModel<bool>> CancelTaskPointAsync(Guid id, CancellationToken ct);

    public Task<ResultModel<bool>> MarkAsDeletedTaskPointAsync(Guid id, CancellationToken ct);

    public Task<ResultModel<ReadModel>> UpdateTaskPointFieldsAsync(UpdateFieldsCommand command, CancellationToken request);
}