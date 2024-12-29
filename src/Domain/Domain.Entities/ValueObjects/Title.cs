using Domain.ValueObjects.BaseValueObjects;
using Domain.ValueObjects.Exceptions.TitleExceptions;
using static Common.Resources.Constants.TitleConstants;

namespace Domain.ValueObjects;

/// <summary>
/// Represents the value of the constraint-checking task header.
/// </summary>
/// <remarks>
/// The class provides validation of the task title bar for the minimum and maximum length, as well as checking for an empty value.
/// </remarks>
/// <param name="value">The value of the task title.</param>
/// <exception cref="TitleEmptyException">Thrown if the task title is empty or consists only of spaces.</exception>
/// <exception cref="TitleMinLengthException">Thrown if the length of the task header is less than the minimum allowed value.</exception>
/// <exception cref="TitleMaxLengthException">Thrown if the length of the task header exceeds the maximum allowed value.</exception>
public class Title(string value) : ValueObject<string>(value)
{

    /// <summary>
    /// Validates the value of the task header.
    /// </summary>
    /// <param name="value">The value to check.</param>
    /// <exception cref="TitleEmptyException">Thrown if the value is empty or consists only of spaces.</exception>
    /// <exception cref="TitleMinLengthException">Thrown if the length is less than the minimum allowed value.</exception>
    /// <exception cref="TitleMaxLengthException">Thrown if the length is greater than the maximum allowed value.</exception>
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