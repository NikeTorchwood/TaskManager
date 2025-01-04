using AutoMapper;
using Domain.Entities;
using Domain.ValueObjects;
using Moq;
using Repositories.Abstractions;
using Services.Contracts.Queries;
using Services.Implementations.Handlers.QueryHandlers;
using Services.Implementations.Mapping;
using Xunit;

namespace Services.Implementations.Tests.QueryHandlersTests;

public class GetAllTaskPointsWithFilterQueryHandlerTests
{
    private readonly Mock<IReadTaskPointsRepository> _repository;
    private readonly GetAllTaskPointsWithFilterQueryHandler _handler;

    private readonly TaskPoint _first =
        new TaskPoint(new Title("First Title"), new Description("First Description"), DateTime.UtcNow.AddDays(2));
    private readonly TaskPoint _second =
        new TaskPoint(new Title("Second Title"), new Description("Second Description"), DateTime.UtcNow.AddDays(2));
    public GetAllTaskPointsWithFilterQueryHandlerTests()
    {
        var config = new MapperConfiguration(
            cfg => cfg.AddProfile<TaskPointsApplicationProfile>());
        var mapper = config.CreateMapper();
        _repository = new Mock<IReadTaskPointsRepository>();
        _handler = new GetAllTaskPointsWithFilterQueryHandler(_repository.Object, mapper);
    }
    [Fact]
    public async Task Handle_NoFilters_ReturnsAllTaskPoints()
    {
        // Arrange
        var taskPoints = new List<TaskPoint>
            {
                _first,
                _second
            }.AsQueryable();

        _repository.Setup(
                repo => repo.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(taskPoints);

        var query = new GetAllTaskPointsWithFilterQuery(_ => true);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        _repository.Verify(repo => repo.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithFilters_ReturnsFilteredTaskPoints()
    {
        // Arrange
        var taskPoints = new List<TaskPoint>
        {
            _first,
            _second
        }.AsQueryable();


        _repository.Setup(
                repo => repo.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(taskPoints);

        var filteredTaskPoints = taskPoints.Where(tp => tp.Title.Value == _first.Title.Value).ToList();


        var query = new GetAllTaskPointsWithFilterQuery(tp => tp.Title.Value == _first.Title.Value);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal(_first.Title.Value, result.First().Title);
        _repository.Verify(repo => repo.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

}