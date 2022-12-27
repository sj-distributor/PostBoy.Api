using System.Net.Http.Headers;
using Microsoft.Extensions.DependencyInjection;
using PostBoy.Core.Data;
using PostBoy.Core.Domain.Authentication;
using PostBoy.Core.Services.Http;
using PostBoy.E2ETests.Mocks;
using Xunit;

namespace PostBoy.E2ETests;

public class ApiKeyFixture : IClassFixture<ApiTestFixture>, IDisposable
{
    
    private readonly ApiTestFixture _factory;

    
    public ApiKeyFixture(ApiTestFixture factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task ShouldBeAbleAuthentication()
    {
        var repo = _factory.Services.GetRequiredService<IRepository>();
        var unitOfWork = _factory.Services.GetRequiredService<IUnitOfWork>();
        
        var userAccountApiKey = new UserAccountApiKey()
        {
            UserAccountId = 123,
            ApiKey = "123"
        };

        await repo.InsertAsync(userAccountApiKey).ConfigureAwait(false);
        await unitOfWork.SaveChangesAsync();

        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Add("apiKey", "123");

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("apiKey", userAccountApiKey.ApiKey);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",
            "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6ImFkbWluIiwibmFtZWlkIjoiMDc1OTI3NDgtYzJkOC00MjcxLWExNWItNmJhZmI4MzAzOWNhIiwibmJmIjoxNjcxNzc1NjkzLCJleHAiOjE2NzE3NzkyOTMsImlhdCI6MTY3MTc3NTY5M30.MSaNNIoApudKpf1UU_l4g8-6rKO2Qb2YBRJxfTNllo4");

        var response = await client.GetAsync($"api/WeChat/apiKeyTest");
        response.EnsureSuccessStatusCode();
    }
    
    public void Dispose()
    {
        _factory.Services.GetRequiredService<InMemoryDbContext>().Database.EnsureDeleted();
    }
}