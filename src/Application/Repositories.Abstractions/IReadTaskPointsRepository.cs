using Domain.Entities;

namespace Repositories.Abstractions;

public interface IReadTaskPointsRepository : IReadRepository<TaskPoint, Guid>;