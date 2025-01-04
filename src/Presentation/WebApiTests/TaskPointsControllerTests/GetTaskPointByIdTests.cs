using Newtonsoft.Json;
using Services.Contracts.Models;
using System.Net;
using WebApi.Responses;
using WebApiTests.TaskPointsControllerTests;
using Xunit;
using static Common.Resources.ResponseErrorMessages.ErrorMessages;

namespace WebApiTests;

public class GetTaskPointByIdTests : IClassFixture<TestFixture>
{
    private readonly HttpClient _client;
    private readonly string _baseRequestUrl = "/api/v1/taskpoints/";
    private readonly string _correctId = "b0ed1cb1-eb8c-4010-91f0-52c709521362";
    public GetTaskPointByIdTests(TestFixture fixture)
    {
        _client = fixture.Client;
    }


    [Fact]
    public async Task GetTaskPointById_ReturnsOkResult_WhenTaskPointExists()
    {
        // Arrange
        var existingId = _correctId;
        var requestUrl = $"{_baseRequestUrl}{existingId}";

        // Act
        var response = await _client.GetAsync(requestUrl);

        // Assert
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<ApiResponse<ReadModel>>(content);

        Assert.NotNull(result);
        Assert.True(result.Success);
        Assert.NotNull(result.Data);
        Assert.Equal(existingId, result.Data.Id.ToString());
    }

    [Fact]
    public async Task GetTaskPointById_ReturnsNotFound_WhenTaskPointDoesNotExist()
    {
        // Arrange
        var nonExistingId = Guid.NewGuid().ToString();
        var requestUrl = $"{_baseRequestUrl}{nonExistingId}";

        // Act
        var response = await _client.GetAsync(requestUrl);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<ApiResponse<string>>(content);

        Assert.NotNull(result);
        Assert.False(result.Success);
        Assert.Equal(ERROR_MESSAGE_TASK_NOT_FOUND, result.ErrorMessage);
    }

    [Fact]
    public async Task GetTaskPointById_ThrowsOperationCanceledException_WhenCancellationTokenTriggered()
    {
        // Arrange
        var validId = _correctId;
        var requestUrl = $"{_baseRequestUrl}{validId}";
        var cts = new CancellationTokenSource();
        cts.Cancel(); // Trigger cancellation

        // Act & Assert
        await Assert.ThrowsAsync<TaskCanceledException>(async () =>
        {
            await _client.GetAsync(requestUrl, cts.Token);
        });
    }
}