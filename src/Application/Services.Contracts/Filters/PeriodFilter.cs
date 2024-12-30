using Domain.Entities;
using System.Linq.Expressions;

namespace Services.Contracts.Filters;

internal class PeriodFilter(
    DateTime? startDateTime,
    DateTime? endDateTime)
    : IFilter<TaskPoint>
{
    public Expression<Func<TaskPoint, bool>> Apply()
        => x =>
            (!startDateTime.HasValue || x.CreatedAt >= startDateTime.Value) &&
                (!endDateTime.HasValue || x.CreatedAt <= endDateTime.Value);
}