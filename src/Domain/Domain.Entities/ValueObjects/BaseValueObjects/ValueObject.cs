namespace Domain.ValueObjects.BaseValueObjects;

/// <summary>
/// The base class of the value object.
/// </summary>
/// <typeparam name="T">Base element.</typeparam>
public abstract class ValueObject<T>
{
    /// <summary>
    /// Value of the base element
    /// </summary>
    public T Value { get; }

    /// <summary>
    /// The basic element of the system
    /// </summary>
    /// <param name="value">The value of the value object.</param>
    protected ValueObject(T value)
    {
        Validate(value);
        Value = value;
    }

    /// <summary>
    /// Validates the value passed to the constructor.
    /// </summary>
    /// <param name="value">The checked value is value object.</param>
    protected abstract void Validate(T value);
}