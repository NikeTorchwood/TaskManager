using Domain.Entities;
using Domain.Entities.Enums;
using Domain.Entities.Exceptions;
using Domain.ValueObjects;
using Xunit;

namespace Domain.Tests.Entities.TaskPointTests;

public class ConstructorTests
{
    private static readonly Title _defaultTitle = new("Default Title");
    private static readonly Description _defaultDescription = new("Default Description");
    private static readonly DateTime _correctDeadline = DateTime.UtcNow.AddDays(1);
    private static readonly DateTime _incorrectDeadline = DateTime.UtcNow.AddHours(-1);

    private static readonly Title? _nullableTitle = null;
    private static readonly Description? _nullableDescription = null;

    public static IEnumerable<object[]> ValidConstructorData =>
        new List<object[]>
        {
                new object[] { _defaultTitle, _defaultDescription, _correctDeadline, false, TaskPointStatuses.Created },
                new object[] { _defaultTitle, _defaultDescription, _correctDeadline, true, TaskPointStatuses.InProgress }
        };

    public static IEnumerable<object[]> InvalidConstructorData =>
        new List<object[]>
        {
                new object[] { _nullableTitle, _defaultDescription, _correctDeadline, typeof(ArgumentNullException) },
                new object[] { _defaultTitle, _nullableDescription, _correctDeadline, typeof(ArgumentNullException) },
                new object[]
                    { _defaultTitle, _defaultDescription, _incorrectDeadline, typeof(CreatingExpiredDeadlineException) }
        };

    [Theory, MemberData(nameof(ValidConstructorData))]
    public void Constructor_ValidParameters_ShouldCreateTaskWithExpectedStatus(
        Title title, Description description, DateTime deadline, bool isStarted, TaskPointStatuses expectedStatus)
    {
        // Act
        var actual = new TaskPoint(
            title,
            description,
            deadline,
            isStarted);

        // Assert
        Assert.NotNull(actual);
        Assert.Equal(title, actual.Title);
        Assert.Equal(description, actual.Description);
        Assert.Equal(deadline, actual.Deadline);
        Assert.Equal(expectedStatus, actual.Status);

        if (isStarted)
            Assert.NotNull(actual.StartedAt);
        else
            Assert.Null(actual.StartedAt);
    }

    [Theory, MemberData(nameof(InvalidConstructorData))]
    public void Constructor_InvalidParameters_ShouldThrowExpectedException(
        Title? title, Description? description, DateTime deadline, Type expectedException)
    {
        // Act
        var actual = () => new TaskPoint(title, description, deadline);

        // Assert
        Assert.Throws(expectedException, actual);
    }

}