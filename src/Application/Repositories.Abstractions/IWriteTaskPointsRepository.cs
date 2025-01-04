using Domain.Entities;

namespace Repositories.Abstractions;

/// <summary>
/// Represents a write repository interface for managing <see cref="TaskPoint"/> entities.
/// </summary>
public interface IWriteTaskPointsRepository : IWriteRepository<TaskPoint, Guid>;