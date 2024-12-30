using Domain.ValueObjects;
using Domain.ValueObjects.Exceptions.TitleExceptions;
using Xunit;
using static Common.Resources.Constants.TitleConstants;

namespace Domain.Tests.ValueObjects;

public class TitleTests
{
    private readonly string _defaultTitle;
    private readonly string _nullableTitle;
    private readonly string _emptyTitle;
    private readonly string _whiteSpacesTitle;
    private readonly string _lessThanMinLengthTitle;
    private readonly string _biggerThanMaxLengthTitle;

    public TitleTests()
    {
        _defaultTitle = "Default Title";
        _nullableTitle = null;
        _emptyTitle = string.Empty;
        _whiteSpacesTitle = "      ";
        _lessThanMinLengthTitle = new string('x', TITLE_MIN_LENGTH - 1);
        _biggerThanMaxLengthTitle = new string('x', TITLE_MAX_LENGTH + 1);
    }

    [Fact]
    public void Constructor_WhenIsValidTitle_ShouldCreatingTitle()
    {
        // Arrange
        var expectedTitle = _defaultTitle;

        // Act
        var actual = new Title(expectedTitle);

        // Assert
        Assert.NotNull(actual);
        Assert.NotNull(actual.Value);
        Assert.Equal(expectedTitle, actual.Value);
    }

    [Fact]
    public void Constructor_WhenTitleIsNull_ShouldThrowTitleEmptyException()
    {
        // Arrange
        var expectedTitle = _nullableTitle;

        // Act
        var actual = () => new Title(expectedTitle);

        // Assert
        Assert.Throws<TitleEmptyException>(actual);
    }

    [Fact]
    public void Constructor_WhenTitleIsEmpty_ShouldThrowTitleEmptyException()
    {
        // Arrange
        var expectedTitle = _emptyTitle;

        // Act
        var actual = () => new Title(expectedTitle);

        // Assert
        Assert.Throws<TitleEmptyException>(actual);
    }

    [Fact]
    public void Constructor_WhenTitleIsWhiteSpaces_ShouldThrowTitleEmptyException()
    {
        // Arrange
        var expectedTitle = _whiteSpacesTitle;

        // Act
        var actual = () => new Title(expectedTitle);

        // Assert
        Assert.Throws<TitleEmptyException>(actual);
    }

    [Fact]
    public void Constructor_WhenTitleLengthIsLessThanMinLengthTitle_ShouldTitleMinLengthException()
    {
        // Arrange
        var expectedTitle = _lessThanMinLengthTitle;

        // Act
        var actual = () => new Title(expectedTitle);

        // Assert
        Assert.Throws<TitleMinLengthException>(actual);
    }

    [Fact]
    public void Constructor_WhenTitleLengthIsBiggerThanMaxLengthTitle_ShouldTitleMaxLengthException()
    {
        // Arrange
        var expectedTitle = _biggerThanMaxLengthTitle;

        // Act
        var actual = () => new Title(expectedTitle);

        // Assert
        Assert.Throws<TitleMaxLengthException>(actual);
    }
}