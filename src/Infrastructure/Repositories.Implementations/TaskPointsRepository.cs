using Domain.Entities;
using EntityFramework;
using Repositories.Abstractions;

namespace Repositories.Implementations;

/// <summary>
/// It provides a read and write repository interface for managing objects<see cref="TaskPoint"/>,
/// which provides common database operations
/// using the Entity Framework for objects with a soft deletion mechanism.
/// </summary>
/// <param name="context">The database context to be used by the repository.</param>
public class TaskPointsRepository(TaskPointsDbContext context)
    : BaseEfRepository<TaskPoint, Guid>(context),
        IReadTaskPointsRepository, IWriteTaskPointsRepository;