using Domain.Entities;
using Domain.ValueObjects;
using Moq;
using Repositories.Abstractions;
using Services.Contracts.Commands;
using Services.Implementations.Handlers.CommandHandlers;
using Xunit;
using static Common.Resources.ResponseErrorMessages.ErrorMessages;

namespace Services.Implementations.Tests.CommandHandlersTests;

public class CompleteTaskPointCommandHandlerTests
{
    private readonly Mock<IWriteTaskPointsRepository> _mockWriteRepository;
    private readonly Mock<IReadTaskPointsRepository> _mockReadRepository;
    private readonly CompleteTaskPointCommandHandler _commandHandler;
    private static readonly Title _title = new("Title");
    private static readonly Description _description = new("Description");
    private static readonly DateTime _deadline = DateTime.UtcNow.AddDays(2);
    public static IEnumerable<object[]> InvalidTaskPointData =>
        new List<object[]>
        {
            new object[]{ _title, _description, _deadline, true, DateTime.UtcNow},
            new object[]{ _title, _description, _deadline, false, null}
        };
    public CompleteTaskPointCommandHandlerTests()
    {
        _mockReadRepository = new Mock<IReadTaskPointsRepository>();
        _mockWriteRepository = new Mock<IWriteTaskPointsRepository>();
        _commandHandler = new CompleteTaskPointCommandHandler(_mockWriteRepository.Object, _mockReadRepository.Object);
    }

    [Fact]
    public async Task Handle_WhenTaskPointNotFound_ShouldReturnFailureResult()
    {
        // Arrange
        var command = new CompleteTaskPointCommand(Guid.NewGuid());
        _mockReadRepository
            .Setup(repo => repo.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((TaskPoint?)null);

        // Act
        var result = await _commandHandler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.Success);
        Assert.Equal(result.Error, ERROR_MESSAGE_TASK_NOT_FOUND);
        _mockWriteRepository.Verify(
            repo => repo.UpdateAsync(
                It.IsAny<TaskPoint>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Theory, MemberData(nameof(InvalidTaskPointData))]
    public async Task Handle_InvalidTaskPointData_ShouldReturnFailureResult(
        Title title, Description description, DateTime deadline, bool isStarted, DateTime closedAt)
    {
        // Arrange
        var taskPoint = new TaskPoint(title, description, deadline, isStarted);
        typeof(TaskPoint)
            .GetProperty(nameof(taskPoint.ClosedAt))
            ?.SetValue(taskPoint, closedAt);
        var command = new CompleteTaskPointCommand(taskPoint.Id);
        _mockReadRepository.Setup(
                repo => repo.GetByIdAsync(taskPoint.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(taskPoint);

        // Act
        var result = await _commandHandler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.Success);
        Assert.Contains(result.Error, new[] { ERROR_MESSAGE_CANT_COMPLETE_CLOSED_TASK, ERROR_MESSAGE_CANT_COMPLETE_NOT_OPENED_TASK });
        _mockWriteRepository.Verify(
            repo => repo.UpdateAsync(
                It.IsAny<TaskPoint>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_ValidTaskPointData_ShouldReturnSuccessResult()
    {
        // Arrange
        var taskPoint = new TaskPoint(_title, _description, _deadline, true);
        var command = new CompleteTaskPointCommand(taskPoint.Id);
        _mockReadRepository.Setup(
                repo => repo.GetByIdAsync(taskPoint.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(taskPoint);

        // Act
        var result = await _commandHandler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.Success);
        Assert.Null(result.Error);
        _mockWriteRepository.Verify(
            repo => repo.UpdateAsync(It.IsAny<TaskPoint>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }
}