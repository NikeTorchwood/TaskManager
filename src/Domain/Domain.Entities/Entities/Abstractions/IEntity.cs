namespace Domain.Entities.Abstractions;

/// <summary>
/// A common interface for entities with a unique identifier.
/// </summary>
/// <typeparam name="TId">The type of the entity ID. Must be a significant type.</typeparam>
public interface IEntity<out TId>
    where TId : struct
{
    /// <summary>
    /// The unique identifier of the entity.
    /// </summary>
    TId Id { get; }
}