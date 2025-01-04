using Domain.Entities;
using Domain.Entities.Enums;
using Domain.Entities.Exceptions;
using Domain.ValueObjects;
using Xunit;

namespace Domain.Tests.Entities.TaskPointTests;

public class CompleteTaskTests
{
    private static DateTime? _nullableClosedAt;
    private static readonly Title _defaultTitle = new("Default Title");
    private static readonly Description _defaultDescription = new("Default Description");
    private static readonly DateTime _correctDeadline = DateTime.UtcNow.AddDays(1);
    private static readonly DateTime _closedAtNow = DateTime.UtcNow;
    public static IEnumerable<object[]> ValidCancelTaskData =>
        new List<object[]>
        {
                new object[]{_defaultTitle, _defaultDescription, _correctDeadline, true}
        };

    public static IEnumerable<object[]> InvalidCancelTaskData =>
        new List<object[]>
        {
                new object[]{_defaultTitle, _defaultDescription, _correctDeadline, true, _closedAtNow, typeof(TaskPointAlreadyClosedException) },
                new object[]{_defaultTitle, _defaultDescription, _correctDeadline, false, _nullableClosedAt, typeof(TaskPointNotStartedException) }
        };

    [Theory, MemberData(nameof(ValidCancelTaskData))]
    public void CancelTask_ValidParameters_ShouldUpdateData(
        Title title, Description description, DateTime deadline, bool isStarted)
    {
        // Arrange
        var taskPoint = new TaskPoint(title, description, deadline, isStarted);
        var expectedStatus = TaskPointStatuses.Completed;

        // Act
        taskPoint.CompleteTask();
        var actualClosedAt = taskPoint.ClosedAt;
        var actualStatus = taskPoint.Status;

        // Assert
        Assert.NotNull(actualClosedAt);
        Assert.Equal(expectedStatus, actualStatus);
    }

    [Theory, MemberData(nameof(InvalidCancelTaskData))]
    public void CancelTask_InvalidParameters_ShouldThrowExpectedException(
        Title title, Description description, DateTime deadline, bool isStarted, DateTime? closedAt, Type expectedException)
    {
        // Arrange
        var taskPoint = new TaskPoint(title, description, deadline, isStarted);
        typeof(TaskPoint)
            .GetProperty(nameof(taskPoint.ClosedAt))
            ?.SetValue(taskPoint, closedAt);

        // Act
        var actual = () => taskPoint.CompleteTask();

        // Assert
        Assert.Throws(expectedException, actual);
    }
}