using Domain.Entities;
using System.Linq.Expressions;

namespace Services.Contracts.Filters;

internal class StatusFilter(
    string status) : IFilter<TaskPoint>
{

    public Expression<Func<TaskPoint, bool>> Apply()
        => x => x.Status.ToString().Equals(status, StringComparison.OrdinalIgnoreCase);
}