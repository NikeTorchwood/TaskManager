using Xunit;

namespace WebApiTests.TaskPointsControllerTests;

public class CompleteTaskPointTests : IClassFixture<TestFixture>
{
    private readonly HttpClient _client;
    private readonly string _baseRequestUrl = "/api/v1/taskpoints/";

    public CompleteTaskPointTests(TestFixture fixture)
    {
        _client = fixture.Client;
    }


}