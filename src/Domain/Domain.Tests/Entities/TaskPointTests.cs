using System.Diagnostics;
using Domain.Entities;
using Domain.Entities.Enums;
using Domain.Entities.Exceptions;
using Domain.ValueObjects;
using Xunit;

namespace Domain.Tests.Entities;

public class TaskPointTests
{
    private readonly Title _defaultTitle;
    private readonly Description _defaultDescription;
    private readonly Title? _nullableTitle;
    private readonly Description? _nullableDescription;
    private readonly DateTime _correctDeadline;
    private readonly DateTime _incorrectDeadline;

    // Конструктор тестового класса
    public TaskPointTests()
    {
        Debug.WriteLine("Test Initialize.");
        _defaultTitle = new Title("Default Task");
        _defaultDescription = new Description("Default Description");
        _nullableTitle = null;
        _nullableDescription = null;
        _correctDeadline = DateTime.UtcNow.AddDays(1);
        _incorrectDeadline = DateTime.UtcNow.AddDays(-1);
    }

    [Fact]
    public void Constructor_WhenIsStartedFalseAndValidParameters_ShouldCreateTaskWithCreatedStatus()
    {
        // Arrange
        var expectedTitle = _defaultTitle;
        var expectedDescription = _defaultDescription;
        var expectedDeadline = _correctDeadline;

        var expectedStatus = TaskPointStatuses.Created;

        // Act
        var actual = new TaskPoint(expectedTitle, expectedDescription, expectedDeadline);

        // Assert
        Assert.NotNull(actual);
        Assert.Equal(expectedTitle, actual.Title);
        Assert.Equal(expectedDescription, actual.Description);
        Assert.Equal(expectedDeadline, actual.Deadline);
        Assert.Equal(expectedStatus, actual.Status);
        Assert.Null(actual.StartedAt);
    }

    [Fact]
    public void Constructor_WhenIsStartedTrueAndValidParameters_ShouldCreateTaskWithInProgressStatus()
    {
        // Arrange
        var expectedTitle = _defaultTitle;
        var expectedDescription = _defaultDescription;
        var expectedDeadline = _correctDeadline;

        var expectedStatus = TaskPointStatuses.InProgress;
        // Act
        var actual = new TaskPoint(expectedTitle, expectedDescription, expectedDeadline, true);

        // Assert
        Assert.NotNull(actual);
        Assert.Equal(expectedTitle, actual.Title);
        Assert.Equal(expectedDescription, actual.Description);
        Assert.Equal(expectedDeadline, actual.Deadline);
        Assert.Equal(expectedStatus, actual.Status);
        Assert.NotNull(actual.StartedAt);
    }

    [Fact]
    public void Constructor_WhenTitleNull_ShouldThrowArgumentException()
    {
        // Arrange
        var expectedTitle = _nullableTitle;
        var expectedDescription = _defaultDescription;
        var expectedDeadline = _correctDeadline; 
        
        // Act
        var actual = () => new TaskPoint(expectedTitle, expectedDescription, expectedDeadline);

        // Assert
        Assert.Throws<ArgumentNullException>(actual);
    }

    [Fact]
    public void Constructor_WhenDescriptionNull_ShouldThrowArgumentException()
    {
        // Arrange
        var expectedTitle = _defaultTitle;
        var expectedDescription = _nullableDescription;
        var expectedDeadline = _correctDeadline;

        // Act
        var actual = () => new TaskPoint(expectedTitle, expectedDescription, expectedDeadline);

        // Assert
        Assert.Throws<ArgumentNullException>(actual);
    }

    [Fact]
    public void Constructor_WhenDeadlineIsEarlierThanCurrentTime_ShouldThrowCreatingExpiredDeadlineException()
    {
        // Arrange
        var expectedTitle = _defaultTitle;
        var expectedDescription = _defaultDescription;
        var expectedDeadline = _incorrectDeadline;

        // Act
        var actual = () => new TaskPoint(expectedTitle, expectedDescription, expectedDeadline);

        // Assert
        Assert.Throws<CreatingExpiredDeadlineException>(actual);
    }

    [Fact]
    public void ChangeTitle_WhenTaskIsNotClosedAndNewTitleIsValid_ShouldUpdateTitle()
    {
        // Arrange
        var taskPoint = new TaskPoint(_defaultTitle, _defaultDescription, _correctDeadline);
        var expected = new Title("New Title");

        // Act
        taskPoint.ChangeTitle(expected);
        var actual = taskPoint.Title;

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ChangeTitle_WhenClosedAtHasValue_ShouldThrowTaskPointEditingForbiddenException()
    {
        // Arrange
        var taskPoint = new TaskPoint(_defaultTitle, _defaultDescription, _correctDeadline);
        typeof(TaskPoint)
            .GetProperty(nameof(taskPoint.ClosedAt))
            ?.SetValue(taskPoint, DateTime.UtcNow);

        var expected = new Title("New Title");

        // Act
        var actual = () => taskPoint.ChangeTitle(expected);

        // Assert
        Assert.Throws<TaskPointEditingForbiddenException>(actual);
    }

    [Fact]
    public void ChangeTitle_WhenNewTitleNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        var taskPoint = new TaskPoint(_defaultTitle, _defaultDescription, _correctDeadline);
        var expected = _nullableTitle;

        // Act
        var actual = () => taskPoint.ChangeTitle(expected);

        // Assert
        Assert.Throws<ArgumentNullException>(actual);
    }

    [Fact]
    public void ChangeDescription_WhenTaskIsNotClosedAndNewDescriptionIsValid_ShouldUpdateDescription()
    {
        // Arrange
        var taskPoint = new TaskPoint(_defaultTitle, _defaultDescription, _correctDeadline);
        var expected = new Description("New Description");

        // Act
        taskPoint.ChangeDescription(expected);
        var actual = taskPoint.Description;

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ChangeDescription_WhenClosedAtHasValue_ShouldThrowTaskPointEditingForbiddenException()
    {
        // Arrange
        var taskPoint = new TaskPoint(_defaultTitle, _defaultDescription, _correctDeadline);
        typeof(TaskPoint)
            .GetProperty(nameof(taskPoint.ClosedAt))
            ?.SetValue(taskPoint, DateTime.UtcNow);

        var expected = new Description("New Description");

        // Act
        var actual = () => taskPoint.ChangeDescription(expected);

        // Assert
        Assert.Throws<TaskPointEditingForbiddenException>(actual);
    }

    [Fact]
    public void ChangeDescription_WhenNewDescriptionNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        var taskPoint = new TaskPoint(_defaultTitle, _defaultDescription, _correctDeadline);
        var expected = _nullableDescription;

        // Act
        var actual = () => taskPoint.ChangeDescription(expected);

        // Assert
        Assert.Throws<ArgumentNullException>(actual);
    }

    [Fact]
    public void ChangeDeadline_WhenTaskIsNotClosedAndNewDeadlineIsValid_ShouldUpdateDescription()
    {
        // Arrange
        var taskPoint = new TaskPoint(_defaultTitle, _defaultDescription, _correctDeadline);
        var expected = _correctDeadline.AddDays(1);

        // Act
        taskPoint.ChangeDeadline(expected);
        var actual = taskPoint.Deadline;

        // Assert
        Assert.Equal(expected, actual);
    }
}