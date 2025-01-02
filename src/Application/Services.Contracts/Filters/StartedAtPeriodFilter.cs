using System.Linq.Expressions;
using Domain.Entities;
using Services.Contracts.Filters;

namespace WebApi.Controllers;

public record StartedAtPeriodFilter(
    DateTime? StartDateTime,
    DateTime? EndDateTime)
    : IFilter<TaskPoint>
{
    public Expression<Func<TaskPoint, bool>> Apply()
        => x =>
            (!StartDateTime.HasValue || x.StartedAt >= StartDateTime.Value) &&
            (!EndDateTime.HasValue || x.StartedAt <= EndDateTime.Value);
}