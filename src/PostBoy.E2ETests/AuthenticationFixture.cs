using System.Net;
using System.Net.Http.Headers;
using Microsoft.Extensions.DependencyInjection;
using PostBoy.Core.Data;
using PostBoy.Core.Domain.Account;
using PostBoy.Core.Domain.Authentication;
using PostBoy.Core.Services.Caching;
using PostBoy.E2ETests.Mocks;
using PostBoy.Messages.DTO.Account;
using Shouldly;
using Xunit;

namespace PostBoy.E2ETests;

[Collection("Sequential")]
public class AuthenticationFixture : IClassFixture<ApiTestFixture>, IDisposable
{
    private readonly HttpClient _client;
    private readonly ApiTestFixture _factory;
    
    public AuthenticationFixture(ApiTestFixture factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task ShouldReturn401CodeWhenGivenErrorApiKey()
    {
        var response = await _client.GetAsync("api/wechat/work/corps");
        
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Theory]
    [InlineData("admin-api-key", "admin-api-key", true)]
    [InlineData("admin-api-key", "admin-api-key-1", false)]
    public async Task ShouldAuthorizedWhenGivenCorrectApiKey(string apiKey, string enrichHeaderKey, bool authorized)
    {
        await InitialApiKeyUser(apiKey);
        
        _client.DefaultRequestHeaders.Add("X-API-KEY", enrichHeaderKey);
        
        var response = await _client.GetAsync("api/wechat/work/corps");

        response.StatusCode.ShouldBe(authorized ? HttpStatusCode.OK : HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task ShouldAuthorizedWhenGivenCorrectJwtToken()
    {
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",
            "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6ImFkbWluIiwibmFtZWlkIjoiMGZjMjE0NDktOGIzNi00NzlkLTgwYWUtYTE3YWZkOTQ4OTAwIiwibmJmIjoxNjcyMTQ5MDIzLCJleHAiOjE2NzIxNTI2MjMsImlhdCI6MTY3MjE0OTAyM30.m08klWNuTdqKULx5SF3UE4oS6o-z969utdBDBLjY4z8");
        
        var response = await _client.GetAsync("api/wechat/work/corps");

        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }
   
    [Fact]
    public async Task ShouldCachingUserWhenAuthenticated()
    {
        var redisCache = _factory.Services.GetRequiredService<RedisCacheService>();
        
        await InitialApiKeyUser();
        
        _client.DefaultRequestHeaders.Add("X-API-KEY", "admin-api-key");
        
        var response = await _client.GetAsync("api/wechat/work/corps");

        var cacheUser = await redisCache.GetAsync<UserAccountDto>("admin-api-key");

        cacheUser.UserName.ShouldBe("admin");
        
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }
    
    private async Task InitialApiKeyUser(string apikey = "admin-api-key")
    {
        var repo = _factory.Services.GetRequiredService<IRepository>();
        var unitOfWork = _factory.Services.GetRequiredService<IUnitOfWork>();

        var userId = Guid.NewGuid();

        await repo.InsertAsync(new UserAccount
        {
            Id = userId,
            UserName = "admin",
            Password = "admin",
            IsActive = true
        });
        await repo.InsertAsync(new UserAccountApiKey
        {
            UserAccountId = userId,
            ApiKey = apikey
        });
        
        await unitOfWork.SaveChangesAsync();
    }
    
    public void Dispose()
    {
        _factory.Services.GetRequiredService<InMemoryDbContext>().Database.EnsureDeleted();
    }
}