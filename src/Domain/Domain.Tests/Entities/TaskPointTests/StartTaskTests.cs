using Domain.Entities;
using Domain.Entities.Enums;
using Domain.Entities.Exceptions;
using Domain.ValueObjects;
using Xunit;

namespace Domain.Tests.Entities.TaskPointTests;

public class StartTaskTests
{
    private static readonly DateTime? _nullableClosedAt = null;
    private static readonly Description _newDefaultDescription = new("New Default Description");
    private static readonly Title _defaultTitle = new("Default Title");
    private static readonly Description _defaultDescription = new("Default Description");
    private static readonly DateTime _correctDeadline = DateTime.UtcNow.AddDays(1);
    private static readonly DateTime _closedAtNow = DateTime.UtcNow;

    private static readonly Description? _nullableDescription = null;

    public static IEnumerable<object[]> ValidStartTaskData =>
        new List<object[]>
        {
                new object[]{_defaultTitle, _defaultDescription, _correctDeadline}
        };

    public static IEnumerable<object[]> InvalidStartTaskData =>
        new List<object[]>
        {
                new object[]{_defaultTitle, _defaultDescription, _correctDeadline, _closedAtNow, typeof(TaskPointAlreadyClosedException) }
        };

    [Theory, MemberData(nameof(ValidStartTaskData))]
    public void StartTask_ValidParameters_ShouldUpdateData(
        Title title, Description description, DateTime deadline)
    {
        // Arrange
        var taskPoint = new TaskPoint(title, description, deadline);
        var expectedStatus = TaskPointStatuses.InProgress;

        // Act
        taskPoint.StartTask();
        var actualStartedAt = taskPoint.StartedAt;
        var actualStatus = taskPoint.Status;

        // Assert
        Assert.NotNull(actualStartedAt);
        Assert.Equal(expectedStatus, actualStatus);
    }

    [Theory, MemberData(nameof(InvalidStartTaskData))]
    public void StartTask_InvalidParameters_ShouldThrowExpectedException(
        Title title, Description description, DateTime deadline, DateTime? closedAt, Type expectedException)
    {
        // Arrange
        var taskPoint = new TaskPoint(title, description, deadline);
        typeof(TaskPoint)
            .GetProperty(nameof(taskPoint.ClosedAt))
            ?.SetValue(taskPoint, closedAt);

        // Act
        var actual = () => taskPoint.StartTask();

        // Assert
        Assert.Throws(expectedException, actual);
    }
}