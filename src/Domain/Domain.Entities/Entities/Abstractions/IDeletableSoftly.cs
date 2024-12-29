namespace Domain.Entities.Abstractions;

/// <summary>
/// Interface for entities that support soft deletion.
/// Soft deletion assumes that the object is not physically deleted, but is marked as deleted.
/// </summary>
public interface IDeletableSoftly
{
    /// <summary>
    /// A flag indicating whether a soft deletion of the object has been performed.
    /// </summary>
    bool IsDeleted { get; }

    /// <summary>
    /// Marks the object as deleted.
    /// </summary>
    void MarkAsDeleted();
}