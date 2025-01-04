using Domain.Entities;
using Domain.ValueObjects;
using Xunit;

namespace Domain.Tests.Entities.TaskPointTests;

public class MarkAsDeletedTests
{
    private static readonly Title _defaultTitle = new("Default Title");
    private static readonly Description _defaultDescription = new("Default Description");
    private static readonly DateTime _correctDeadline = DateTime.UtcNow.AddDays(1);

    [Fact]
    public void MarkAsDeleted_WhenIsDeletedFalse_ShouldUpdatedIsDeleted()
    {
        // Arrange
        var taskPoint = new TaskPoint(_defaultTitle, _defaultDescription, _correctDeadline);
        var expected = true;

        // Act
        taskPoint.MarkAsDeleted();
        var actual = taskPoint.IsDeleted;

        // Assert
        Assert.Equal(expected, actual);
    }
}