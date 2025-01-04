using Domain.Entities;
using Domain.ValueObjects;
using Moq;
using Repositories.Abstractions;
using Services.Contracts.Commands;
using Services.Implementations.Handlers.CommandHandlers;
using Xunit;
using static Common.Resources.ResponseErrorMessages.ErrorMessages;

namespace Services.Implementations.Tests.CommandHandlersTests;
public class MarkAsDeletedCommandHandlerTests
{
    private readonly Mock<IWriteTaskPointsRepository> _mockWriteRepository;
    private readonly Mock<IReadTaskPointsRepository> _mockReadRepository;
    private readonly MarkAsDeletedCommandHandler _commandHandler;
    private readonly Title _title = new("Title");
    private readonly Description _description = new("Description");
    private readonly DateTime _deadline = DateTime.UtcNow.AddDays(2);
    public MarkAsDeletedCommandHandlerTests()
    {
        _mockReadRepository = new Mock<IReadTaskPointsRepository>();
        _mockWriteRepository = new Mock<IWriteTaskPointsRepository>();
        _commandHandler = new MarkAsDeletedCommandHandler(_mockWriteRepository.Object, _mockReadRepository.Object);
    }

    [Fact]
    public async Task Handle_ValidParameters_ReturnSuccessResult()
    {
        // Arrange
        var taskPoint = new TaskPoint(_title, _description, _deadline);
        var command = new MarkAsDeletedCommand(taskPoint.Id);
        _mockReadRepository.Setup(
                repo => repo.GetByIdAsync(taskPoint.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(taskPoint);

        // Act
        var result = await _commandHandler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.Success);
        Assert.True(result.Value);
        Assert.Null(result.Error);
        _mockWriteRepository.Verify(
            repo => repo.UpdateAsync(taskPoint, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WhenTaskPointNotFound_ReturnFailureResult()
    {
        // Arrange
        var command = new MarkAsDeletedCommand(Guid.NewGuid());
        _mockReadRepository.Setup(
                repo => repo.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((TaskPoint?)null);

        // Act
        var result = await _commandHandler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.Success);
        Assert.Equal(result.Error, ERROR_MESSAGE_TASK_NOT_FOUND);
        _mockWriteRepository.Verify(
            repo => repo.UpdateAsync(It.IsAny<TaskPoint>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }
}