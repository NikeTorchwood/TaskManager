using Newtonsoft.Json;
using Services.Contracts.Models;
using System.Net;
using System.Text;
using WebApi.Requests;
using WebApi.Responses;
using Xunit;

namespace WebApiTests.TaskPointsControllerTests;

public class CreateTaskPointTests : IClassFixture<TestFixture>
{
    private readonly HttpClient _client;
    private readonly string _baseRequestUrl = "/api/v1/taskpoints/";

    public CreateTaskPointTests(TestFixture fixture)
    {
        _client = fixture.Client;
    }

    [Fact]
    public async Task CreateTaskPoint_ReturnsCreatedResult_WhenRequestIsValid()
    {
        // Arrange
        var request =
            new CreatingTaskPointRequest(
                Title: "New Task",
                Description: "Description of the task",
                Deadline: DateTime.UtcNow.AddDays(7),
                IsStarted: false);

        var requestContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync(_baseRequestUrl, requestContent);

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<ApiResponse<ReadModel>>(content);

        Assert.NotNull(result);
        Assert.True(result.Success);
        Assert.Equal(request.Title, result.Data.Title);
        Assert.Equal(request.Description, result.Data.Description);
    }

    [Fact]
    public async Task CreateTaskPoint_ReturnsBadRequest_WhenRequestIsInvalid()
    {
        // Arrange
        var request =
            new CreatingTaskPointRequest(
                Title: "",
                Description: "Description of the task",
                Deadline: DateTime.UtcNow.AddDays(7),
                IsStarted: false);

        var requestContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync(_baseRequestUrl, requestContent);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<ApiResponse<string>>(content);

        Assert.NotNull(result);
        Assert.False(result.Success);
        Assert.NotNull(result.ErrorMessage);
    }

    [Fact]
    public async Task CreateTaskPoint_ReturnsBadRequest_WhenDeadlineIsInThePast()
    {
        // Arrange
        var request =
            new CreatingTaskPointRequest(
                Title: "New Task",
                Description: "Description of the task",
                Deadline: DateTime.UtcNow.AddDays(-1),
                IsStarted: false);

        var requestContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync(_baseRequestUrl, requestContent);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<ApiResponse<string>>(content);

        Assert.NotNull(result);
        Assert.False(result.Success);
        Assert.NotNull(result.ErrorMessage);
    }

    [Fact]
    public async Task CreateTaskPoint_HandlesCancellationTokenCorrectly()
    {
        // Arrange
        var request =
            new CreatingTaskPointRequest(
                Title: "New Task",
                Description: "Description of the task",
                Deadline: DateTime.UtcNow.AddDays(7),
                IsStarted: false);

        var requestContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
        using var cts = new CancellationTokenSource();
        cts.Cancel();

        // Act & Assert
        await Assert.ThrowsAsync<TaskCanceledException>(() =>
            _client.PostAsync(_baseRequestUrl, requestContent, cts.Token));
    }
}