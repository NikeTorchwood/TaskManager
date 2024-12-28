namespace Domain.Entities.Abstractions;

/// <summary>
/// Общий интерфейс для сущностей с уникальным идентификатором.
/// </summary>
/// <typeparam name="TId">Тип идентификатора сущности. Должен быть значимым типом.</typeparam>
public interface IEntity<out TId>
    where TId : struct
{
    /// <summary>
    /// Уникальный идентификатор сущности.
    /// </summary>
    TId Id { get; }
}