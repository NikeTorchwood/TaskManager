using Domain.Entities;
using Domain.ValueObjects;
using Moq;
using Repositories.Abstractions;
using Services.Contracts.Commands;
using Services.Implementations.Handlers.CommandHandlers;
using Xunit;
using static Common.Resources.ResponseErrorMessages.ErrorMessages;

namespace Services.Implementations.Tests.CommandHandlersTests;
public class CancelTaskPointCommandHandlerTests
{
    private readonly Mock<IWriteTaskPointsRepository> _mockWriteRepository;
    private readonly Mock<IReadTaskPointsRepository> _mockReadRepository;
    private readonly CancelTaskPointCommandHandler _handler;
    private readonly Title _title = new("Title");
    private readonly Description _description = new("Description");
    private readonly DateTime _deadline = DateTime.UtcNow.AddDays(2);
    public CancelTaskPointCommandHandlerTests()
    {
        _mockWriteRepository = new Mock<IWriteTaskPointsRepository>();
        _mockReadRepository = new Mock<IReadTaskPointsRepository>();
        _handler = new CancelTaskPointCommandHandler(_mockWriteRepository.Object, _mockReadRepository.Object);
    }

    [Fact]
    public async Task Handle_WhenTaskPointNotFound_ShouldReturnFailureResult()
    {
        // Arrange
        var command = new CancelTaskPointCommand(Guid.NewGuid());
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
    public async Task Handle_WhenTaskPointIsAlreadyClosed_ShouldReturnFailureResult()
    {
        // Arrange
        var taskPoint = new TaskPoint(_title, _description, _deadline);
        taskPoint.CancelTask();
        var command = new CancelTaskPointCommand(taskPoint.Id);

        _mockReadRepository
            .Setup(repo => repo.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(taskPoint);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.Success);
        Assert.Equal(ERROR_MESSAGE_TASK_ALREADY_CLOSED, result.Error);
        _mockWriteRepository.Verify(
            repo => repo.UpdateAsync(It.IsAny<TaskPoint>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_WhenTaskPointIsValid_ShouldReturnSuccessResult()
    {
        // Arrange
        var taskPoint = new TaskPoint(_title, _description, _deadline);
        var command = new CancelTaskPointCommand(taskPoint.Id);

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