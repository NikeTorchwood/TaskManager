using System.Linq.Expressions;

namespace Services.Contracts.Filters;

/// <summary>
/// Represents a filter interface for applying filtering logic to entities of type <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">The type of the entity to be filtered.</typeparam>
public interface IFilter<T>
{
    /// <summary>
    /// Applies the filter expression to an entity of type <typeparamref name="T"/>.
    /// </summary>
    /// <returns>An <see cref="Expression{Func{T, bool}}"/> representing the filtering logic.</returns>
    Expression<Func<T, bool>> Apply();
}