using Services.Contracts.Commands;
using Services.Contracts.Models;

namespace Services.Abstractions;

public interface ITaskPointService
{
    public Task<ResultModel<ReadModel>> GetTaskPointByIdAsync(Guid id, CancellationToken ct);

    public Task<IEnumerable<ReadModel>> GetAllTaskPointsWithFilterAsync(FilterModel filter, CancellationToken ct);

    public Task<ResultModel<ReadModel>> CreateTaskPointAsync(CreateTaskPointCommand command, CancellationToken ct);

    //public Task ChangeTaskPointTitleAsync(ChangeTitleModel model, CancellationToken ct);

    //public Task ChangeTaskPointDescriptionAsync(ChangeDescriptionModel model, CancellationToken ct);

    //public Task ChangeTaskPointDeadlineAsync(ChangeDeadlineModel model, CancellationToken ct);

    //public Task StartTaskPointAsync(Guid id, CancellationToken ct);

    //public Task CompleteTaskPointAsync(Guid id, CancellationToken ct);

    //public Task CancelTaskPointAsync(Guid id, CancellationToken ct);

    //public Task MarkAsDeletedTaskPointAsync(Guid id, CancellationToken ct);
}