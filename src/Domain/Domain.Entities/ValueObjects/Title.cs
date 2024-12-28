using Domain.ValueObjects.BaseValueObjects;
using Domain.ValueObjects.Exceptions.TitleExceptions;
using static Common.Resources.Constants.TitleConstants;

namespace Domain.ValueObjects;

/// <summary>
/// Представляет значение заголовка задачи с проверкой ограничений.
/// </summary>
/// <remarks>
/// Класс обеспечивает валидацию строки заголовка задачи на минимальную и максимальную длину, а также проверку на пустое значение.
/// </remarks>
/// <param name="value">Значение заголовка задачи.</param>
/// <exception cref="TitleEmptyException">Выбрасывается, если заголовок задачи пустой или состоит только из пробелов.</exception>
/// <exception cref="TitleMinLengthException">Выбрасывается, если длина заголовка задачи меньше минимально допустимого значения.</exception>
/// <exception cref="TitleMaxLengthException">Выбрасывается, если длина заголовка задачи превышает максимально допустимое значение.</exception>
public class Title(string value) : ValueObject<string>(value)
{

    /// <summary>
    /// Выполняет валидацию значения заголовка задачи.
    /// </summary>
    /// <param name="value">Значение, которое нужно проверить.</param>
    /// <exception cref="TitleEmptyException">Если значение пустое или состоит только из пробелов.</exception>
    /// <exception cref="TitleMinLengthException">Если длина меньше минимально допустимого значения.</exception>
    /// <exception cref="TitleMaxLengthException">Если длина больше максимально допустимого значения.</exception>
    protected override void Validate(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new TitleEmptyException(nameof(value));

        if (value.Length < TITLE_MIN_LENGTH)
            throw new TitleMinLengthException(value.Length, nameof(value));

        if (value.Length > TITLE_MAX_LENGTH)
            throw new TitleMaxLengthException(value.Length, nameof(value));
    }
}