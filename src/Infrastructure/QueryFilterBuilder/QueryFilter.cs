using System.Linq.Expressions;
using static System.Linq.Expressions.Expression;

namespace QueryFilterBuilder;

public class QueryFilter<T>(Expression<Func<T, bool>> expression)
{
    public Expression<Func<T, bool>> Expression { get; } = expression
                                                           ?? throw new ArgumentNullException(nameof(expression));

    public Expression<Func<T, bool>> Combine(
        QueryFilter<T> other,
        ParameterExpression sharedParameter)
    {
        if (other == null)
            throw new ArgumentNullException(nameof(other));

        var body = AndAlso(
            Invoke(Expression, sharedParameter),
            Invoke(other.Expression, sharedParameter)
        );

        return Lambda<Func<T, bool>>(body, sharedParameter);
    }
}