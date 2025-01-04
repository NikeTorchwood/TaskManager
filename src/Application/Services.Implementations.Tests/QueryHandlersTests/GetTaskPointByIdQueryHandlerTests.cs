using AutoMapper;
using Domain.Entities;
using Domain.ValueObjects;
using Moq;
using Repositories.Abstractions;
using Services.Contracts.Queries;
using Services.Implementations.Handlers.QueryHandlers;
using Services.Implementations.Mapping;
using Xunit;
using static Common.Resources.ResponseErrorMessages.ErrorMessages;

namespace Services.Implementations.Tests.QueryHandlersTests;

public class GetTaskPointByIdQueryHandlerTests
{
    private readonly Mock<IReadTaskPointsRepository> _repository;
    private readonly GetTaskPointByIdQueryHandler _handler;

    public GetTaskPointByIdQueryHandlerTests()
    {
        _repository = new Mock<IReadTaskPointsRepository>();

        // Set up AutoMapper with the required profiles
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<TaskPointsApplicationProfile>();
        });

        var mapper = config.CreateMapper();

        _handler = new GetTaskPointByIdQueryHandler(_repository.Object, mapper);
    }

    [Fact]
    public async Task Handle_WhenTaskPointExists_ShouldReturnSuccessResult()
    {
        // Arrange
        var taskPoint = new TaskPoint(
            new Title("Sample Task"),
            new Description("Task Description"),
            DateTime.UtcNow.AddDays(2));

        _repository
            .Setup(repo => repo.GetByIdAsync(taskPoint.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(taskPoint);

        var query = new GetTaskPointByIdQuery(taskPoint.Id);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.Success);
        Assert.NotNull(result.Value);
        Assert.Null(result.Error);

        Assert.Equal(taskPoint.Id, result.Value.Id);
        Assert.Equal(taskPoint.Title.Value, result.Value.Title);
        Assert.Equal(taskPoint.Description.Value, result.Value.Description);
        Assert.Equal(taskPoint.Deadline, result.Value.Deadline);

        _repository.Verify(repo => repo.GetByIdAsync(taskPoint.Id, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenTaskPointDoesNotExist_ShouldReturnFailureResult()
    {
        // Arrange
        var taskPointId = Guid.NewGuid();

        _repository
            .Setup(repo => repo.GetByIdAsync(taskPointId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Domain.Entities.TaskPoint?)null);

        var query = new GetTaskPointByIdQuery(taskPointId);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.False(result.Success);
        Assert.Null(result.Value);
        Assert.Equal(ERROR_MESSAGE_TASK_NOT_FOUND, result.Error);

        _repository.Verify(repo => repo.GetByIdAsync(taskPointId, It.IsAny<CancellationToken>()), Times.Once);
    }
}
