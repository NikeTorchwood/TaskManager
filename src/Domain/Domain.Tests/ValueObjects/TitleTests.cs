using Domain.ValueObjects;
using Domain.ValueObjects.Exceptions.TitleExceptions;
using Xunit;
using static Common.Resources.Constants.TitleConstants;

namespace Domain.Tests.ValueObjects;

public class TitleTests
{
    private readonly string _defaultTitle = "Default Title";
    private static readonly string _nullableTitle = null;
    private static readonly string _emptyTitle = string.Empty;
    private static readonly string _whiteSpacesTitle = new(' ', TITLE_MIN_LENGTH + 1);
    private static readonly string _lessThanMinLengthTitle = new('x', TITLE_MIN_LENGTH - 1);
    private static readonly string _biggerThanMaxLengthTitle = new('x', TITLE_MAX_LENGTH + 1);

    public static IEnumerable<object[]> InvalidDescriptionData =>
        new List<object[]>
        {
            new object[]{_nullableTitle, typeof(TitleEmptyException)},
            new object[]{_emptyTitle, typeof(TitleEmptyException) },
            new object[]{_whiteSpacesTitle, typeof(TitleEmptyException) },
            new object[]{_lessThanMinLengthTitle, typeof(TitleMinLengthException)},
            new object[]{_biggerThanMaxLengthTitle, typeof(TitleMaxLengthException)}
        };

    [Fact]
    public void Constructor_ValidParameters_ShouldCreateTitle()
    {
        // Arrange
        var expected = _defaultTitle;

        // Act
        var actual = new Title(expected);

        // Assert
        Assert.NotNull(actual);
        Assert.NotNull(actual.Value);
        Assert.Equal(expected, actual.Value);
    }

    [Theory, MemberData(nameof(InvalidDescriptionData))]
    public void Constructor_InvalidParameters_ShouldThrowExpectedException(string title, Type expectedException)
    {
        // Arrange
        var expectedTitle = title;

        // Act
        var actual = () => new Title(expectedTitle);

        // Assert
        Assert.Throws(expectedException, actual);
    }
}