using System.Net.Http.Headers;
using Microsoft.Extensions.DependencyInjection;
using PostBoy.Core.Services.Http;
using PostBoy.E2ETests.Mocks;
using Xunit;

namespace PostBoy.E2ETests;

public class ApiKeyFixture : IClassFixture<ApiTestFixture>, IDisposable
{
    
    private readonly ApiTestFixture _factory;
    private readonly PostBoyHttpClientFactory _httpClientFactory;
    
    public ApiKeyFixture(ApiTestFixture factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task ShouldBeAbleAuthentication()
    {
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Add("X-API-KEY","123");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6ImFkbWluIiwibmFtZWlkIjoiMDc1OTI3NDgtYzJkOC00MjcxLWExNWItNmJhZmI4MzAzOWNhIiwibmJmIjoxNjcxNzc1NjkzLCJleHAiOjE2NzE3NzkyOTMsImlhdCI6MTY3MTc3NTY5M30.MSaNNIoApudKpf1UU_l4g8-6rKO2Qb2YBRJxfTNllo4");
        
        var response = await client.GetAsync($"api/wechat/work/corps");
        response.EnsureSuccessStatusCode();
    }
    
    public void Dispose()
    {
        _factory.Services.GetRequiredService<InMemoryDbContext>().Database.EnsureDeleted();
    }
}