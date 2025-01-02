using Domain.Entities;
using EntityFramework;
using Repositories.Abstractions;

namespace Repositories.Implementations;

public class TaskPointsRepository(TaskPointsDbContext context)
    : BaseEfRepository<TaskPoint, Guid>(context),
        IReadTaskPointsRepository, IWriteTaskPointsRepository;