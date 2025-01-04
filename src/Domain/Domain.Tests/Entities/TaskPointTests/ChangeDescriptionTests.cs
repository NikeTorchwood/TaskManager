using Domain.Entities;
using Domain.Entities.Exceptions;
using Domain.ValueObjects;
using Xunit;

namespace Domain.Tests.Entities.TaskPointTests;

public class ChangeDescriptionTests
{
    private static readonly DateTime? _nullableClosedAt = null;
    private static readonly Description _newDefaultDescription = new("New Default Description");
    private static readonly Title _defaultTitle = new("Default Title");
    private static readonly Description _defaultDescription = new("Default Description");
    private static readonly DateTime _correctDeadline = DateTime.UtcNow.AddDays(1);
    private static readonly DateTime _closedAtNow = DateTime.UtcNow;

    private static readonly Description? _nullableDescription = null;

    public static IEnumerable<object[]> ValidChangeDescriptionData =>
        new List<object[]>
        {
                new object[]{_defaultTitle, _defaultDescription, _correctDeadline, _newDefaultDescription}
        };

    public static IEnumerable<object[]> InvalidChangeDescriptionData =>
        new List<object[]>
        {
                new object[]{_defaultTitle, _defaultDescription, _correctDeadline, _nullableDescription, _nullableClosedAt, typeof(ArgumentNullException)},
                new object[]{_defaultTitle, _defaultDescription, _correctDeadline, _newDefaultDescription, _closedAtNow, typeof(TaskPointEditingForbiddenException) }
        };

    [Theory, MemberData(nameof(ValidChangeDescriptionData))]
    public void ChangeDescription_ValidParameters_ShouldUpdateData(
        Title title, Description description, DateTime deadline, Description newDescription)
    {
        // Arrange
        var taskPoint = new TaskPoint(title, description, deadline);
        var expected = newDescription;

        // Act
        taskPoint.ChangeDescription(expected);
        var actual = taskPoint.Description;

        // Assert
        Assert.Equal(expected, actual);
    }

    [Theory, MemberData(nameof(InvalidChangeDescriptionData))]
    public void ChangeDescription_InvalidParameters_ShouldThrowExpectedException(
        Title title, Description description, DateTime deadline, Description newDescription, DateTime? closedAt, Type expectedException)
    {
        // Arrange
        var taskPoint = new TaskPoint(title, description, deadline);
        typeof(TaskPoint)
            .GetProperty(nameof(taskPoint.ClosedAt))
            ?.SetValue(taskPoint, closedAt);

        // Act
        var actual = () => taskPoint.ChangeDescription(newDescription);

        // Assert
        Assert.Throws(expectedException, actual);
    }
}