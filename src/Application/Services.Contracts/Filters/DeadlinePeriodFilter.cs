using System.Linq.Expressions;
using Domain.Entities;
using Services.Contracts.Filters;

namespace WebApi.Controllers;

public record DeadlinePeriodFilter(
    DateTime? StartDateTime,
    DateTime? EndDateTime)
    : IFilter<TaskPoint>
{
    public Expression<Func<TaskPoint, bool>> Apply()
        => x =>
            (!StartDateTime.HasValue || x.Deadline >= StartDateTime.Value) &&
            (!EndDateTime.HasValue || x.Deadline <= EndDateTime.Value);
}