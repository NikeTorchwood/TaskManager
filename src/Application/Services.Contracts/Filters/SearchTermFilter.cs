using Domain.Entities;
using System.Linq.Expressions;

namespace Services.Contracts.Filters;

/// <summary>
/// Represents a filter that filters <see cref="TaskPoint"/> entities based on a search term.
/// </summary>
/// <param name="SearchTerm">The search term to be used for filtering the entities.</param>
public record SearchTermFilter(string SearchTerm) : IFilter<TaskPoint>
{
    /// <summary>
    /// Applies the filter expression to a <see cref="TaskPoint"/> entity.
    /// </summary>
    /// <returns>An <see cref="Expression{Func{TaskPoint, bool}}"/> representing the filtering logic based on the search term.</returns>
    public Expression<Func<TaskPoint, bool>> Apply()
        => x => (x.Description != null && x.Description.Value != null &&
                 x.Description.Value.Contains(SearchTerm)) ||
                (x.Title != null && x.Title.Value != null &&
                 x.Title.Value.Contains(SearchTerm));
}