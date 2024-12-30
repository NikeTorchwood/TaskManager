using Domain.Entities;

namespace Repositories.Abstractions;

public interface IWriteTaskPointsRepository : IWriteRepository<TaskPoint, Guid>;