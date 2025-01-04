using Domain.Entities;
using Domain.ValueObjects;
using Moq;
using Repositories.Abstractions;
using Services.Contracts.Commands;
using Services.Implementations.Handlers.CommandHandlers;
using Xunit;
using static Common.Resources.ResponseErrorMessages.ErrorMessages;

namespace Services.Implementations.Tests.CommandHandlersTests;

public class StartTaskPointCommandHandlerTests
{
    private readonly Mock<IWriteTaskPointsRepository> _mockWriteRepository;
    private readonly Mock<IReadTaskPointsRepository> _mockReadRepository;
    private readonly StartTaskPointCommandHandler _handler;
    private static readonly Title _title = new("Title");
    private static readonly Description _description = new("Description");
    private static readonly DateTime _deadline = DateTime.UtcNow.AddDays(2);
    public static IEnumerable<object[]> InvalidTaskPointData =>
        new List<object[]>
        {
            new object[]{ _title, _description, _deadline, false, DateTime.UtcNow},
            new object[]{ _title, _description, _deadline, true, null}
        };
    public StartTaskPointCommandHandlerTests()
    {
        _mockWriteRepository = new Mock<IWriteTaskPointsRepository>();
        _mockReadRepository = new Mock<IReadTaskPointsRepository>();
        _handler = new StartTaskPointCommandHandler(_mockWriteRepository.Object, _mockReadRepository.Object);
    }

    [Theory, MemberData(nameof(InvalidTaskPointData))]
    public async Task Handle_InvalidTaskPointData_ShouldReturnFailureResult(
        Title title, Description description, DateTime deadline, bool isStarted, DateTime? closedAt)
    {
        // Arrange
        var taskPoint = new TaskPoint(title, description, deadline, isStarted);
        typeof(TaskPoint)
            .GetProperty(nameof(taskPoint.ClosedAt))
            ?.SetValue(taskPoint, closedAt);

        var command = new StartTaskPointCommand(taskPoint.Id);
        _mockReadRepository
            .Setup(repo => repo.GetByIdAsync(taskPoint.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(taskPoint);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.Success);
        Assert.Contains(result.Error, new[] { ERROR_MESSAGE_CANT_START_CLOSED_TASK, ERROR_MESSAGE_TASK_ALREADY_STARTED });
        _mockWriteRepository.Verify(
            repo => repo.UpdateAsync(It.IsAny<TaskPoint>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_WhenTaskPointNotFound_ShouldReturnFailureResult()
    {
        // Arrange
        var command = new StartTaskPointCommand(Guid.NewGuid());
        _mockReadRepository
            .Setup(repo => repo.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((TaskPoint?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.Success);
        Assert.Equal(result.Error, ERROR_MESSAGE_TASK_NOT_FOUND);
        _mockWriteRepository.Verify(
            repo => repo.UpdateAsync(It.IsAny<TaskPoint>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_WhenTaskPointIsValid_ShouldCancelTaskPoint()
    {
        // Arrange
        var taskPoint = new TaskPoint(_title, _description, _deadline);
        var command = new StartTaskPointCommand(taskPoint.Id);

        _mockReadRepository
            .Setup(repo => repo.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(taskPoint);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.Success);
        Assert.True(result.Value);
        Assert.Null(result.Error);
        _mockWriteRepository.Verify(
            repo => repo.UpdateAsync(taskPoint, It.IsAny<CancellationToken>()),
            Times.Once);
    }
}