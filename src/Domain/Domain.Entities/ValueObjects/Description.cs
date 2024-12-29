using Domain.ValueObjects.BaseValueObjects;
using Domain.ValueObjects.Exceptions.DescriptionsExceptions;
using static Common.Resources.Constants.DescriptionConstants;

namespace Domain.ValueObjects;

/// <summary>
/// Represents the value of the constraint-checking task description.
/// </summary>
/// <remarks>
/// The class provides validation of the task description string for the minimum and maximum length, as well as checking for an empty value.
/// </remarks>
/// <param name="value">The value of the task description.</param>
/// <exception cref="DescriptionEmptyException">Thrown if the task description is empty or consists only of spaces.</exception>
/// <exception cref="DescriptionMinLengthException">Thrown if the length of the task description is less than the minimum allowed value.</exception>
/// <exception cref="DescriptionMaxLengthException">Thrown if the task description is longer than the maximum allowed value.</exception>
public class Description(string value) : ValueObject<string>(value)
{
    /// <summary>
    /// Performs validation of the description value.
    /// </summary>
    /// <param name="value">The value that needs to be checked.</param>
    /// <exception cref="DescriptionEmptyException">Thrown if the value is empty or consists only of spaces.</exception>
    /// <exception cref="DescriptionMinLengthException">Thrown if the length is less than the minimum allowed value.</exception>
    /// <exception cref="DescriptionMaxLengthException">Thrown if the length is greater than the maximum allowed value.</exception>
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
