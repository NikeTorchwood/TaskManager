using Domain.Entities.Enums;
using Newtonsoft.Json;
using Services.Contracts.Models;
using System.Net;
using Xunit;

namespace WebApiTests.TaskPointsControllerTests;

public class GetAllWithFilterTests : IClassFixture<TestFixture>
{
    private readonly HttpClient _client;
    private readonly string _baseRequestUrl = "/api/v1/taskpoints/";

    public GetAllWithFilterTests(TestFixture fixture)
    {
        _client = fixture.Client;
    }

    [Fact]
    public async Task GetAllWithFilter_ReturnsAllTaskPoints_WhenNoFiltersProvided()
    {
        // Arrange
        var requestUrl = $"{_baseRequestUrl}";

        // Act
        var response = await _client.GetAsync(requestUrl);

        // Assert
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<IEnumerable<ReadModel>>(content);

        Assert.NotNull(result);
        Assert.NotEmpty(result);
    }

    [Fact]
    public async Task GetAllWithFilter_ReturnsFilteredTaskPoints_WhenTaskPointStatusFilterProvided()
    {
        // Arrange
        var status = TaskPointStatuses.InProgress;
        var requestUrl = $"{_baseRequestUrl}?TaskPointStatus={status}";

        // Act
        var response = await _client.GetAsync(requestUrl);

        // Assert
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<IEnumerable<ReadModel>>(content);

        Assert.NotNull(result);
        Assert.All(result, taskPoint => Assert.Equal(status, taskPoint.Status));
    }

    [Fact]
    public async Task GetAllWithFilter_ReturnsFilteredTaskPoints_WhenMultipleFiltersProvided()
    {
        // Arrange
        var status = TaskPointStatuses.InProgress;
        var deadlineStart = DateTime.UtcNow.AddDays(-7).ToString("o");
        var deadlineEnd = DateTime.UtcNow.AddDays(7).ToString("o");
        var requestUrl = $"{_baseRequestUrl}?TaskPointStatus={status}&DeadlineStartPeriod={deadlineStart}&DeadlineEndPeriod={deadlineEnd}";

        // Act
        var response = await _client.GetAsync(requestUrl);

        // Assert
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<IEnumerable<ReadModel>>(content);

        Assert.NotNull(result);
        Assert.All(result, taskPoint =>
        {
            Assert.Equal(status, taskPoint.Status);
            Assert.True(taskPoint.Deadline >= DateTime.Parse(deadlineStart) && taskPoint.Deadline <= DateTime.Parse(deadlineEnd));
        });
    }

    [Fact]
    public async Task GetAllWithFilter_ReturnsBadRequest_WhenInvalidTaskPointStatusProvided()
    {
        // Arrange
        var requestUrl = $"{_baseRequestUrl}?TaskPointStatus=InvalidStatus";

        // Act
        var response = await _client.GetAsync(requestUrl);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetAllWithFilter_ReturnsEmptyResult_WhenNoTaskPointsMatchFilters()
    {
        // Arrange
        var searchTerm = "NonExistentTask";
        var requestUrl = $"{_baseRequestUrl}?SearchTerm={searchTerm}";

        // Act
        var response = await _client.GetAsync(requestUrl);

        // Assert
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<IEnumerable<ReadModel>>(content);

        Assert.NotNull(result);
        Assert.Empty(result);
    }
}