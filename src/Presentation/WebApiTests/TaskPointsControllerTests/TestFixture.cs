using Microsoft.AspNetCore.Mvc.Testing;
using WebApi;

namespace WebApiTests.TaskPointsControllerTests;

public class TestFixture
{
    public HttpClient Client { get; private set; }

    public TestFixture()
    {
        WebApplicationFactory<Program> factory = new();
        Client = factory.CreateClient();
        Console.WriteLine("Fixture initialized");
    }
}