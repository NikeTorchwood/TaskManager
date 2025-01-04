using AutoMapper;
using Domain.Entities;
using Moq;
using Repositories.Abstractions;
using Services.Contracts.Commands;
using Services.Implementations.Handlers.CommandHandlers;
using Xunit;
using static Common.Resources.ResponseErrorMessages.ErrorMessages;

namespace Services.Implementations.Tests.CommandHandlersTests;

public class CreateTaskPointCommandHandlerTests
{
    private readonly Mock<IWriteTaskPointsRepository> _mockWriteRepository;
    private readonly CreateTaskPointCommandHandler _commandHandler;

    public CreateTaskPointCommandHandlerTests()
    {
        var config = new MapperConfiguration(
            cfg => cfg.AddProfile<Mapping.TaskPointsApplicationProfile>());
        var mapper = config.CreateMapper();
        _mockWriteRepository = new Mock<IWriteTaskPointsRepository>();
        _commandHandler = new CreateTaskPointCommandHandler(_mockWriteRepository.Object, mapper);
    }

    public static IEnumerable<object[]> ValidCreateTaskPointData = new List<object[]>
    {
        new object[] { "Title", "Description", DateTime.UtcNow.AddDays(2), false },
        new object[] { "Title", "Description", DateTime.UtcNow.AddDays(2), true }
    };

    public static IEnumerable<object[]> InValidCreateTaskPointData = new List<object[]>
    {
        new object[] { string.Empty, "Description", DateTime.UtcNow.AddDays(2), false},
        new object[] { "Title", string.Empty, DateTime.UtcNow.AddDays(2), false},
        new object[] { "Title", "Description", DateTime.UtcNow.AddDays(-1), false}
    };


    [Theory, MemberData(nameof(ValidCreateTaskPointData))]
    public async Task Handle_ValidParameters_ShouldReturnSuccessResult(
            string title, string description, DateTime deadline, bool isStarted)
    {
        // Arrange
        var command = new CreateTaskPointCommand(title, description, deadline, isStarted);
        TaskPoint? capturedTaskPoint = null;
        _mockWriteRepository.Setup(
                repo => repo.AddAsync(It.IsAny<TaskPoint>(), It.IsAny<CancellationToken>()))
            .Callback<TaskPoint, CancellationToken>((taskPoint, ct) => capturedTaskPoint = taskPoint)
            .ReturnsAsync((TaskPoint taskPoint, CancellationToken ct) => taskPoint);

        // Act
        var result = await _commandHandler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.Success);
        Assert.NotNull(result.Value);
        Assert.Null(result.Error);
        _mockWriteRepository.Verify(
            repo => repo.AddAsync(It.IsAny<TaskPoint>(), It.IsAny<CancellationToken>()),
            Times.Once);

        Assert.NotNull(capturedTaskPoint);
        Assert.Equal(capturedTaskPoint.Title.Value, title);
        Assert.Equal(capturedTaskPoint.Description.Value, description);
        Assert.Equal(capturedTaskPoint.Deadline, deadline);
        Assert.NotNull(capturedTaskPoint.CreatedAt);
    }

    [Theory, MemberData(nameof(InValidCreateTaskPointData))]
    public async Task Handle_InvalidParameters_ShouldReturnFailureResult(
        string title, string description, DateTime deadline, bool isStarted)
    {
        // Arrange
        var command = new CreateTaskPointCommand(title, description, deadline, isStarted);
        _mockWriteRepository.Setup(
                repo => repo.AddAsync(It.IsAny<TaskPoint>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((TaskPoint taskPoint, CancellationToken ct) => taskPoint);

        // Act
        var result = await _commandHandler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.Success);
        Assert.Null(result.Value);
        Assert.NotNull(result.Error);
        Assert.Contains(result.Error, new[] { ERROR_MESSAGE_INVALID_DATA, ERROR_MESSAGE_DEADLINE_MUST_BE_IN_FUTURE });
        _mockWriteRepository.Verify(
            repo => repo.UpdateAsync(
                It.IsAny<TaskPoint>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }
}
