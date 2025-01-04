using Domain.Entities;
using Domain.Entities.Exceptions;
using Domain.ValueObjects;
using Xunit;

namespace Domain.Tests.Entities.TaskPointTests;

public class ChangeDeadlineTests
{
    private static readonly DateTime? _nullableClosedAt = null;
    private static readonly DateTime _newCorrectDeadline = DateTime.UtcNow.AddDays(2);
    private static readonly Title _defaultTitle = new("Default Title");
    private static readonly Description _defaultDescription = new("Default Description");
    private static readonly DateTime _correctDeadline = DateTime.UtcNow.AddDays(1);
    private static readonly DateTime _deadlineInPast = DateTime.UtcNow.AddDays(-1);
    private static readonly DateTime _closedAtNow = DateTime.UtcNow;

    public static IEnumerable<object[]> ValidChangeDeadlineData =>
        new List<object[]>
        {
                new object[]{_defaultTitle, _defaultDescription, _correctDeadline, _newCorrectDeadline}
        };

    public static IEnumerable<object[]> InvalidChangeDeadlineData =>
        new List<object[]>
        {
                new object[]{_defaultTitle, _defaultDescription, _correctDeadline, _deadlineInPast, _nullableClosedAt, typeof(CreatingExpiredDeadlineException)},
                new object[]{_defaultTitle, _defaultDescription, _correctDeadline, _newCorrectDeadline, _closedAtNow, typeof(TaskPointEditingForbiddenException)}
        };

    [Theory, MemberData(nameof(ValidChangeDeadlineData))]
    public void ChangeDeadline_ValidParameters_ShouldUpdateData(
        Title title, Description description, DateTime deadline, DateTime newDeadline)
    {
        // Arrange
        var taskPoint = new TaskPoint(title, description, deadline);
        var expected = newDeadline;

        // Act
        taskPoint.ChangeDeadline(expected);
        var actual = taskPoint.Deadline;

        // Assert
        Assert.Equal(expected, actual);
    }

    [Theory, MemberData(nameof(InvalidChangeDeadlineData))]
    public void ChangeDeadline_InvalidParameters_ShouldThrowExpectedException(
        Title title, Description description, DateTime deadline, DateTime newDeadline, DateTime? closedAt, Type expectedException)
    {
        // Arrange
        var taskPoint = new TaskPoint(title, description, deadline);
        typeof(TaskPoint)
            .GetProperty(nameof(taskPoint.ClosedAt))
            ?.SetValue(taskPoint, closedAt);

        // Act
        var actual = () => taskPoint.ChangeDeadline(newDeadline);

        // Assert
        Assert.Throws(expectedException, actual);
    }
}