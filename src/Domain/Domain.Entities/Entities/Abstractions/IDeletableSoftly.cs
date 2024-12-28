namespace Domain.Entities.Abstractions;

/// <summary>
/// Интерфейс для сущностей, поддерживающих мягкое удаление.
/// Мягкое удаление предполагает, что объект не удаляется физически, а помечается как удалённый.
/// </summary>
public interface IDeletableSoftly
{
    /// <summary>
    /// Флаг, указывающий, было ли выполнено мягкое удаление объекта.
    /// </summary>
    bool IsDeleted { get; }

    /// <summary>
    /// Помечает объект как удалённый.
    /// </summary>
    void MarkAsDeleted();
}