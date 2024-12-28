namespace Domain.ValueObjects.BaseValueObjects;

/// <summary>
/// Базовый класс объекта значения
/// </summary>
/// <typeparam name="T">Тип-обобщение базового элемента</typeparam>
public abstract class ValueObject<T>
{
    /// <summary>
    /// Значение базового элемента
    /// </summary>
    public T Value { get; }

    /// <summary>
    /// Базовый элемент системы
    /// </summary>
    /// <param name="value">Значение, которое хранится в элементе и проходит валидацию</param>
    protected ValueObject(T value)
    {
        Validate(value);
        Value = value;
    }

    /// <summary>
    /// Валидирует значение переданное в конструктор
    /// </summary>
    /// <param name="value">Значение, которое хранится в элементе и проходит валидацию</param>
    protected abstract void Validate(T value);
}