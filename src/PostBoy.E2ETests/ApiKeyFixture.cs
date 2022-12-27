using System.Net;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using PostBoy.Core.Data;
using PostBoy.Core.Domain.Authentication;
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
    public async Task ShouldBeAbleSuccessAuthentication()
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
        
        var request = new HttpRequestMessage(HttpMethod.Post, "api/Message/send");
        
        request.Headers.Add("X-API-KEY","1234");

        var client = _factory.CreateClient();
        
        request.Content = new StringContent("{}", Encoding.UTF8, "application/json");
        
        var responseOk = await client.SendAsync(request);
        
        Assert.True(responseOk.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.OK, responseOk.StatusCode);
    }

    public void Dispose()
    {
        _factory.Services.GetRequiredService<InMemoryDbContext>().Database.EnsureDeleted();
    }
}