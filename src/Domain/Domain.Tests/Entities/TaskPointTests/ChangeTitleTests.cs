using Domain.Entities;
using Domain.Entities.Exceptions;
using Domain.ValueObjects;
using Xunit;

namespace Domain.Tests.Entities.TaskPointTests;

public class ChangeTitleTests
{
    private static readonly DateTime? _nullableClosedAt = null;
    private static readonly Title _newDefaultTitle = new("New Default Title");
    private static readonly Title _defaultTitle = new("Default Title");
    private static readonly Description _defaultDescription = new("Default Description");
    private static readonly DateTime _correctDeadline = DateTime.UtcNow.AddDays(1);
    private static readonly DateTime _closedAtNow = DateTime.UtcNow;

    private static readonly Title? _nullableTitle = null;

    public static IEnumerable<object[]> ValidChangeTitleData =>
        new List<object[]>
        {
                new object[]{_defaultTitle, _defaultDescription, _correctDeadline, _newDefaultTitle}
        };

    public static IEnumerable<object[]> InvalidChangeTitleData =>
        new List<object[]>
        {
                new object[]{_defaultTitle, _defaultDescription, _correctDeadline, _nullableTitle, _nullableClosedAt, typeof(ArgumentNullException)},
                new object[]{_defaultTitle, _defaultDescription, _correctDeadline, _newDefaultTitle, _closedAtNow, typeof(TaskPointEditingForbiddenException) }
        };

    [Theory, MemberData(nameof(ValidChangeTitleData))]
    public void ChangeTitle_ValidParameters_ShouldUpdateData(
        Title title, Description description, DateTime deadline, Title newTitle)
    {
        // Arrange
        var taskPoint = new TaskPoint(title, description, deadline);
        var expected = newTitle;

        // Act
        taskPoint.ChangeTitle(newTitle);
        var actual = taskPoint.Title;

        // Assert
        Assert.Equal(expected, actual);
    }

    [Theory, MemberData(nameof(InvalidChangeTitleData))]
    public void ChangeTitle_InvalidParameters_ShouldThrowExpectedException(
        Title title, Description description, DateTime deadline, Title newTitle, DateTime? closedAt, Type expectedException)
    {
        // Arrange
        var taskPoint = new TaskPoint(title, description, deadline);
        typeof(TaskPoint)
            .GetProperty(nameof(taskPoint.ClosedAt))
            ?.SetValue(taskPoint, closedAt);

        // Act
        var actual = () => taskPoint.ChangeTitle(newTitle);

        // Assert
        Assert.Throws(expectedException, actual);
    }
}