using Domain.ValueObjects;
using Domain.ValueObjects.Exceptions.DescriptionsExceptions;
using Xunit;
using static Common.Resources.Constants.DescriptionConstants;

namespace Domain.Tests.ValueObjects;
public class DescriptionTests
{
    private readonly string _defaultDescription = "Default Description";
    private static readonly string _nullableDescription = null;
    private static readonly string _emptyDescription = string.Empty;
    private static readonly string _whiteSpacesDescription = new(' ', DESCRIPTION_MIN_LENGTH + 1);
    private static readonly string _lessThanMinLengthDescription = new('x', DESCRIPTION_MIN_LENGTH - 1);
    private static readonly string _biggerThanMaxLengthDescription = new('x', DESCRIPTION_MAX_LENGTH + 1);

    public static IEnumerable<object[]> InvalidDescriptionData =>
        new List<object[]>
        {
            new object[]{_nullableDescription, typeof(DescriptionEmptyException)},
            new object[]{_emptyDescription, typeof(DescriptionEmptyException)},
            new object[]{_whiteSpacesDescription, typeof(DescriptionEmptyException)},
            new object[]{_lessThanMinLengthDescription, typeof(DescriptionMinLengthException)},
            new object[]{_biggerThanMaxLengthDescription, typeof(DescriptionMaxLengthException)}
        };

    [Fact]
    public void Constructor_ValidParameters_ShouldCreateDescription()
    {
        // Arrange
        var expected = _defaultDescription;

        // Act
        var actual = new Description(expected);

        // Assert
        Assert.NotNull(actual);
        Assert.NotNull(actual.Value);
        Assert.Equal(expected, actual.Value);
    }

    [Theory, MemberData(nameof(InvalidDescriptionData))]
    public void Constructor_InvalidParameters_ShouldThrowExpectedException(string description, Type expectedException)
    {
        // Arrange
        var expectedDescription = description;

        // Act
        var actual = () => new Description(expectedDescription);

        // Assert
        Assert.Throws(expectedException, actual);
    }
}