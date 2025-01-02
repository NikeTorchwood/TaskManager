using System.Linq.Expressions;

namespace QueryFilterBuilder;

public class QueryFilterBuilder<T>
{
    private Expression<Func<T, bool>> _combinedExpression = x => true;

    public QueryFilterBuilder<T> AddFilter(Expression<Func<T, bool>> filter)
    {
        if (filter == null) return this;

        var parameter = Expression.Parameter(typeof(T));
        var body = Expression.AndAlso(
            Expression.Invoke(_combinedExpression, parameter),
            Expression.Invoke(filter, parameter)
        );

        _combinedExpression = Expression.Lambda<Func<T, bool>>(body, parameter);

        return this;
    }
    public Expression<Func<T, bool>> Build()
    {
        return _combinedExpression;
    }
}