using Newtonsoft.Json;
using Services.Contracts.Models;
using System.Net;
using System.Net.Http.Json;
using WebApi.Requests;
using WebApi.Responses;
using Xunit;
using static Common.Resources.ResponseErrorMessages.ErrorMessages;

namespace WebApiTests.TaskPointsControllerTests;

public class MarkAsDeletedTests : IClassFixture<TestFixture>
{
    private readonly HttpClient _client;
    private readonly string _baseRequestUrl = "/api/v1/taskpoints/";

    public MarkAsDeletedTests(TestFixture fixture)
    {
        _client = fixture.Client;
    }

    [Fact]
    public async Task MarkAsDeletedTaskPoint_ReturnsNoContent_WhenTaskPointExists()
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
        var deleteResponse = await _client.DeleteAsync($"{_baseRequestUrl}{createdTaskId}");

        // Assert
        deleteResponse.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);
    }

    [Fact]
    public async Task MarkAsDeletedTaskPoint_ReturnsNotFound_WhenTaskPointDoesNotExist()
    {
        // Arrange
        var nonExistentTaskId = Guid.NewGuid();  // ID, который не существует в базе
        var requestUrl = $"{_baseRequestUrl}{nonExistentTaskId}";

        // Act
        var response = await _client.DeleteAsync(requestUrl);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

        var content = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<ApiResponse<string>>(content);
        Assert.NotNull(result);
        Assert.False(result.Success);
        Assert.Equal(ERROR_MESSAGE_TASK_NOT_FOUND, result.ErrorMessage);
    }
}