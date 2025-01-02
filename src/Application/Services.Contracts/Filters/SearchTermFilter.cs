using System.Linq.Expressions;
using Domain.Entities;

namespace Services.Contracts.Filters;

public record SearchTermFilter(string SearchTerm) : IFilter<TaskPoint>
{
    public Expression<Func<TaskPoint, bool>> Apply()
        => x => (x.Description != null && x.Description.Value != null &&
                 x.Description.Value.Contains(SearchTerm)) ||
                (x.Title != null && x.Title.Value != null &&
                 x.Title.Value.Contains(SearchTerm));
}