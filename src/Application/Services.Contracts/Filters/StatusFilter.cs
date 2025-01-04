using Domain.Entities;
using Domain.Entities.Enums;
using System.Linq.Expressions;

namespace Services.Contracts.Filters;

/// <summary>
/// Represents a filter that filters <see cref="TaskPoint"/> entities based on their status.
/// </summary>
public record StatusFilter(TaskPointStatuses? Status) : IFilter<TaskPoint>
{
    /// <summary>
    /// Applies the filter expression to a <see cref="TaskPoint"/> entity.
    /// </summary>
    /// <returns>An <see cref="Expression{Func{TaskPoint, bool}}"/> representing the filtering logic based on the status.</returns>
    public Expression<Func<TaskPoint, bool>> Apply()
        => x => x.Status == Status;
}