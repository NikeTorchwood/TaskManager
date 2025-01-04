using Domain.Entities;
using System.Linq.Expressions;

namespace Services.Contracts.Filters;

/// <summary>
/// Represents a filter that filters <see cref="TaskPoint"/> entities based on their deadline date range.
/// </summary>
/// <param name="StartDateTime">The start date and time for filtering (optional).</param>
/// <param name="EndDateTime">The end date and time for filtering (optional).</param>
public record DeadlinePeriodFilter(
    DateTime? StartDateTime,
    DateTime? EndDateTime)
    : IFilter<TaskPoint>
{
    /// <summary>
    /// Applies the filter expression to a <see cref="TaskPoint"/> entity.
    /// </summary>
    /// <returns>An <see cref="Expression{Func{TaskPoint, bool}}"/> representing the filtering logic based on <see cref="TaskPoint.Deadline"/> date range.</returns>
    public Expression<Func<TaskPoint, bool>> Apply()
        => x =>
            (!StartDateTime.HasValue || x.Deadline >= StartDateTime.Value) &&
            (!EndDateTime.HasValue || x.Deadline <= EndDateTime.Value);
}