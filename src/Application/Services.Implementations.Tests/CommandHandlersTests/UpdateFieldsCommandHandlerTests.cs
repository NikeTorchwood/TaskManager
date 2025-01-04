using AutoMapper;
using Domain.Entities;
using Domain.ValueObjects;
using Moq;
using Repositories.Abstractions;
using Services.Contracts.Commands;
using Services.Implementations.Handlers.CommandHandlers;
using Services.Implementations.Mapping;
using Xunit;
using static Common.Resources.Constants.DescriptionConstants;
using static Common.Resources.Constants.TitleConstants;
using static Common.Resources.ResponseErrorMessages.ErrorMessages;

namespace Services.Implementations.Tests.CommandHandlersTests;

public class UpdateFieldsCommandHandlerTests
{
    private readonly Mock<IWriteTaskPointsRepository> _mockWriteRepository;
    private readonly Mock<IReadTaskPointsRepository> _mockReadRepository;
    private readonly UpdateFieldsCommandHandler _commandHandler;
    private static readonly string _newTitle = "New Title";
    private static readonly string _newDescription = "New Description";
    private static readonly DateTime _newDeadline = DateTime.UtcNow.AddDays(2);
    private static readonly string _shortNewTitle = new('x', TITLE_MIN_LENGTH - 1);
    private static readonly string _longNewTitle = new('x', TITLE_MAX_LENGTH + 1);
    private static readonly string _shortNewDescription = new('x', DESCRIPTION_MIN_LENGTH - 1);
    private static readonly string _longNewDescription = new('x', DESCRIPTION_MAX_LENGTH + 1);
    private static readonly DateTime _deadlineInPast = DateTime.UtcNow.AddDays(-1);


    private static readonly TaskPoint _taskPoint = new(
            new Title("Title"),
            new Description("Description"),
            DateTime.UtcNow.AddDays(2));


    public static readonly IEnumerable<object[]> ValidTaskPointData = new List<object[]>
    {
        new object[] { _taskPoint, _newTitle, _newDescription, _newDeadline },
        new object[] {_taskPoint, _newTitle, null, null},
        new object[] {_taskPoint, _newTitle, _newDescription, null},
        new object[] {_taskPoint, _newTitle, null, _newDeadline},
        new object[] {_taskPoint, null , _newDescription, null},
        new object[] {_taskPoint, null , _newDescription, _newDeadline},
        new object[] {_taskPoint, null, null, _newDeadline},
    };
    public static readonly IEnumerable<object[]> InvalidTaskPointData = new List<object[]>
    {
        new object[] { _taskPoint, _newTitle, _newDescription, _newDeadline , false, DateTime.UtcNow},
        new object[] { _taskPoint, _newTitle, _newDescription, _newDeadline , true, null},
        new object[] { _taskPoint, _shortNewTitle, _newDescription, _newDeadline , false, null},
        new object[] { _taskPoint, _longNewTitle, _newDescription, _newDeadline , false, null},
        new object[] { _taskPoint, _newTitle, _shortNewDescription, _newDeadline , false, null},
        new object[] { _taskPoint, _newTitle, _longNewDescription, _newDeadline , false, null},
        new object[] { _taskPoint, _newTitle, _newDescription, _deadlineInPast , false, null},
    };

    public UpdateFieldsCommandHandlerTests()
    {
        var config = new MapperConfiguration(
            cfg => cfg.AddProfile<TaskPointsApplicationProfile>());
        var mapper = config.CreateMapper();
        _mockReadRepository = new Mock<IReadTaskPointsRepository>();
        _mockWriteRepository = new Mock<IWriteTaskPointsRepository>();
        _commandHandler = new UpdateFieldsCommandHandler(_mockReadRepository.Object, _mockWriteRepository.Object, mapper);
    }

    [Theory, MemberData(nameof(ValidTaskPointData))]
    public async Task Handle_WhenValidData_ShouldReturnSuccessResult(
        TaskPoint taskPoint, string newTitle, string newDescription, DateTime? newDeadline)
    {
        // Arrange
        var expectedTitle = string.IsNullOrWhiteSpace(newTitle)
            ? taskPoint.Title.Value
            : newTitle;
        var expectedDescription = string.IsNullOrWhiteSpace(newDescription)
            ? taskPoint.Description.Value
            : newDescription;
        var expectedDeadline = newDeadline ?? taskPoint.Deadline;

        var command = new UpdateFieldsCommand(taskPoint.Id, newTitle, newDescription, newDeadline);
        TaskPoint? capturedTaskPoint = null;
        _mockReadRepository.Setup(
                repo => repo.GetByIdAsync(taskPoint.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(taskPoint);
        _mockWriteRepository.Setup(
                repo => repo.UpdateAsync(It.IsAny<TaskPoint>(), It.IsAny<CancellationToken>()))
            .Callback<TaskPoint, CancellationToken>((updatedTaskPoint, ct) => capturedTaskPoint = updatedTaskPoint)
            .Returns(Task.CompletedTask);

        // Act
        var result = await _commandHandler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.Success);
        Assert.NotNull(result.Value);
        Assert.Null(result.Error);
        _mockWriteRepository.Verify(
            repo => repo.UpdateAsync(It.IsAny<TaskPoint>(), It.IsAny<CancellationToken>()),
            Times.Once);

        Assert.NotNull(capturedTaskPoint);
        Assert.Equal(capturedTaskPoint.Title.Value, expectedTitle);
        Assert.Equal(capturedTaskPoint.Description.Value, expectedDescription);
        Assert.Equal(capturedTaskPoint.Deadline, expectedDeadline);
    }

    [Theory, MemberData(nameof(InvalidTaskPointData))]
    public async Task Handle_WhenInvalidData_ShouldReturnFailureResult(
        TaskPoint taskPoint, string newTitle, string newDescription, DateTime? newDeadline, bool isDeleted, DateTime? closedAt)
    {
        // Arrange
        typeof(TaskPoint)
            .GetProperty(nameof(taskPoint.IsDeleted))
            ?.SetValue(taskPoint, isDeleted);
        typeof(TaskPoint)
            .GetProperty(nameof(taskPoint.ClosedAt))
            ?.SetValue(taskPoint, closedAt);
        var command = new UpdateFieldsCommand(taskPoint.Id, newTitle, newDescription, newDeadline);
        TaskPoint? capturedTaskPoint = null;
        _mockReadRepository.Setup(
                repo => repo.GetByIdAsync(taskPoint.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(taskPoint);

        // Act
        var result = await _commandHandler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.Success);
        Assert.Contains(result.Error, new[]
        {
            ERROR_MESSAGE_TASK_ALREADY_CLOSED,
            ERROR_MESSAGE_TASK_WAS_DELETED,
            ERROR_MESSAGE_TITLE_SHORTER_MIN_LENGTH,
            ERROR_MESSAGE_TITLE_LONGER_MAX_LENGTH,
            ERROR_MESSAGE_DESCRIPTION_SHORTER_MIN_LENGTH,
            ERROR_MESSAGE_DESCRIPTION_LONGER_MAX_LENGTH,
            ERROR_MESSAGE_DEADLINE_MUST_BE_IN_FUTURE
        });
        _mockWriteRepository.Verify(
            repo => repo.UpdateAsync(It.IsAny<TaskPoint>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }
}