using Domain.Entities;

namespace Repositories.Abstractions;

/// <summary>
/// Represents a read-only repository interface for accessing <see cref="TaskPoint"/> entities.
/// </summary>
public interface IReadTaskPointsRepository : IReadRepository<TaskPoint, Guid>;