using Domain.Entities;
using System.Linq.Expressions;

namespace Services.Contracts.Filters;

internal class SearchTermFilter(string searchTerm) : IFilter<TaskPoint>
{
    public Expression<Func<TaskPoint, bool>> Apply()
        => x => x.Description.ToString().Contains(searchTerm) ||
                x.Title.ToString().Contains(searchTerm);
}