using Domain.ValueObjects.BaseValueObjects;
using Domain.ValueObjects.Exceptions.DescriptionsExceptions;
using static Common.Resources.Constants.DescriptionConstants;

namespace Domain.ValueObjects;

/// <summary>
/// Представляет значение описания задачи с проверкой ограничений.
/// </summary>
/// <remarks>
/// Класс обеспечивает валидацию строки описания задачи на минимальную и максимальную длину, а также проверку на пустое значение.
/// </remarks>
/// <param name="value">Значение описания задачи.</param>
/// <exception cref="DescriptionEmptyException">Выбрасывается, если описание задачи пусто или состоит только из пробелов.</exception>
/// <exception cref="DescriptionMinLengthException">Выбрасывается, если длина описания задачи меньше минимально допустимого значения.</exception>
/// <exception cref="DescriptionMaxLengthException">Выбрасывается, если длина описания задачи превышает максимально допустимое значение.</exception>
public class Description(string value) : ValueObject<string>(value)
{
    /// <summary>
    /// Выполняет валидацию значения описания.
    /// </summary>
    /// <param name="value">Значение, которое нужно проверить.</param>
    /// <exception cref="DescriptionEmptyException">Если значение пустое или состоит только из пробелов.</exception>
    /// <exception cref="DescriptionMinLengthException">Если длина меньше минимально допустимого значения.</exception>
    /// <exception cref="DescriptionMaxLengthException">Если длина больше максимально допустимого значения.</exception>
    protected override void Validate(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DescriptionEmptyException(nameof(value));

        if (value.Length < DESCRIPTION_MIN_LENGTH)
            throw new DescriptionMinLengthException(value.Length, nameof(value));

        if (value.Length > DESCRIPTION_MAX_LENGTH)
            throw new DescriptionMaxLengthException(value.Length, nameof(value));
    }

}
