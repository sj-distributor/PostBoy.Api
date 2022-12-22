using Microsoft.Extensions.DependencyInjection;
using PostBoy.E2ETests.Mocks;
using Xunit;

namespace PostBoy.E2ETests;

public class ApiKeyFixture : IClassFixture<ApiTestFixture>, IDisposable
{
    private readonly HttpClient _client;
    private readonly ApiTestFixture _factory;
    
    public ApiKeyFixture(HttpClient client, ApiTestFixture factory)
    {
        _client = client;
        _factory = factory;
    }


    
    public void Dispose()
    {
        _factory.Services.GetRequiredService<InMemoryDbContext>().Database.EnsureDeleted();
    }
}