using System.Linq.Expressions;

namespace QueryFilterBuilder;

public class QueryFilterBuilder<T>
{
    private readonly ParameterExpression _sharedParameter = Expression.Parameter(typeof(T), "x");
    private Expression<Func<T, bool>> _combinedExpression = x => true;

    public QueryFilterBuilder<T> AddFilter(Expression<Func<T, bool>> filter)
    {
        if (filter == null) return this;

        var newFilter = new QueryFilter<T>(filter);
        var currentFilter = new QueryFilter<T>(_combinedExpression);

        _combinedExpression = currentFilter.Combine(newFilter, _sharedParameter);
        return this;
    }
    public Expression<Func<T, bool>> Build()
    {
        return _combinedExpression;
    }
}