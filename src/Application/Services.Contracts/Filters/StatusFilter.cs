using Domain.Entities;
using System.Linq.Expressions;
using Domain.Entities.Enums;

namespace Services.Contracts.Filters;

public record StatusFilter(
    TaskPointStatuses? Status) : IFilter<TaskPoint>
{

    public Expression<Func<TaskPoint, bool>> Apply()
        => x => x.Status == Status;
}