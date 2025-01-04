using System.Linq.Expressions;

namespace QueryFilterBuilder;

/// <summary>
/// A builder class for combining multiple filter expressions into a single expression.
/// </summary>
/// <typeparam name="T">The type of the entity that the filter expressions will be applied to.</typeparam>
public class QueryFilterBuilder<T>
{
    /// <summary>
    /// The combined expression that represents all added filters.
    /// Initially, it is a true condition (no filtering).
    /// </summary>
    private Expression<Func<T, bool>> _combinedExpression = x => true;

    /// <summary>
    /// Adds a filter expression to the combined expression using a logical AND.
    /// </summary>
    /// <param name="filter">The filter expression to be added.</param>
    /// <returns>The current instance of <see cref="QueryFilterBuilder{T}"/> to allow method chaining.</returns>
    /// <remarks>
    /// If the provided <paramref name="filter"/> is null, no modification will be made to the combined expression.
    /// </remarks>
    public QueryFilterBuilder<T> AddFilter(Expression<Func<T, bool>> filter)
    {
        if (filter == null) return this;

        // Creates a new parameter for the lambda expression to ensure it's valid in the current context.
        var parameter = Expression.Parameter(typeof(T));

        // Combines the current expression with the new filter using logical AND.
        var body = Expression.AndAlso(
            Expression.Invoke(_combinedExpression, parameter),
            Expression.Invoke(filter, parameter)
        );

        // Updates the combined expression with the new filter.
        _combinedExpression = Expression.Lambda<Func<T, bool>>(body, parameter);

        return this;
    }

    /// <summary>
    /// Builds and returns the combined filter expression.
    /// </summary>
    /// <returns>The combined filter expression.</returns>
    /// <remarks>
    /// The resulting expression can be used to query data with all the filters that have been added.
    /// </remarks>
    public Expression<Func<T, bool>> Build()
    {
        return _combinedExpression;
    }
}