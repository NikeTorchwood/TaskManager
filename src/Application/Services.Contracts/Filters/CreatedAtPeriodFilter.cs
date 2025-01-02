using Domain.Entities;
using System.Linq.Expressions;

namespace Services.Contracts.Filters;

public record CreatedAtPeriodFilter(
    DateTime? StartDateTime,
    DateTime? EndDateTime)
    : IFilter<TaskPoint>
{
    public Expression<Func<TaskPoint, bool>> Apply()
        => x =>
            (!StartDateTime.HasValue || x.CreatedAt >= StartDateTime.Value) &&
                (!EndDateTime.HasValue || x.CreatedAt <= EndDateTime.Value);
}