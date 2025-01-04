using Newtonsoft.Json;
using Services.Contracts.Models;
using System.Net;
using System.Net.Http.Json;
using WebApi.Requests;
using WebApi.Responses;
using Xunit;
using static Common.Resources.ResponseErrorMessages.ErrorMessages;

namespace WebApiTests.TaskPointsControllerTests;

public class CancelTaskPointTests : IClassFixture<TestFixture>
{
    private readonly HttpClient _client;
    private readonly string _baseRequestUrl = "/api/v1/taskpoints/";

    public CancelTaskPointTests(TestFixture fixture)
    {
        _client = fixture.Client;
    }
    [Fact]
    public async Task CancelTaskPoint_ReturnsNoContent_WhenTaskPointExists()
    {
        // Arrange
        var createResponse = await _client.PostAsJsonAsync(_baseRequestUrl, new CreatingTaskPointRequest(
            Title: "New Task",
            Description: "Description of the task",
            Deadline: DateTime.UtcNow.AddDays(7),
            IsStarted: false));

        createResponse.EnsureSuccessStatusCode();
        var createdTaskString = await createResponse.Content.ReadAsStringAsync();
        var createdTask = JsonConvert.DeserializeObject<ApiResponse<ReadModel>>(createdTaskString);
        var createdTaskId = createdTask.Data.Id;

        // Act
        var cancelResponse = await _client.PostAsync($"{_baseRequestUrl}{createdTaskId}/Cancel", null);

        // Assert
        cancelResponse.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.NoContent, cancelResponse.StatusCode);
    }

    [Fact]
    public async Task CancelTaskPoint_ReturnsNotFound_WhenTaskPointDoesNotExist()
    {
        // Arrange
        var nonExistentTaskId = Guid.NewGuid();
        var requestUrl = $"{_baseRequestUrl}{nonExistentTaskId}/Cancel";

        // Act
        var response = await _client.PostAsync(requestUrl, null);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

        var content = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<ApiResponse<string>>(content);
        Assert.NotNull(result);
        Assert.False(result.Success);
        Assert.Equal(ERROR_MESSAGE_TASK_NOT_FOUND, result.ErrorMessage);
    }

    [Fact]
    public async Task CancelTaskPoint_ReturnsBadRequest_WhenTaskPointHasInvalidData()
    {
        // Arrange
        var createResponse = await _client.PostAsJsonAsync(_baseRequestUrl, new CreatingTaskPointRequest(
            Title: "New Task",
            Description: "Description of the task",
            Deadline: DateTime.UtcNow.AddDays(7),
            IsStarted: false));

        createResponse.EnsureSuccessStatusCode();
        var createdTaskString = await createResponse.Content.ReadAsStringAsync();
        var createdTask = JsonConvert.DeserializeObject<ApiResponse<ReadModel>>(createdTaskString);
        var createdTaskId = createdTask.Data.Id;
        var cancelResponse = await _client.PostAsync($"{_baseRequestUrl}{createdTaskId}/Cancel", null);

        // Act
        var response = await _client.PostAsync($"{_baseRequestUrl}{createdTaskId}/Cancel", null);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var content = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<ApiResponse<string>>(content);
        Assert.NotNull(result);
        Assert.False(result.Success);
        Assert.Equal(ERROR_MESSAGE_TASK_ALREADY_CLOSED, result.ErrorMessage);
    }
}
