using PostBoy.Core.Services.Caching;
using PostBoy.IntegrationTests.TestBaseClasses;
using Shouldly;
using Xunit;

namespace PostBoy.IntegrationTests.Services.Caching;

public class RedisCachingFixture : GenericFixtureBase
{
    [Fact]
    public async Task ShouldObjectTypeCacheWork()
    {
        await Run<RedisCacheService>(async cachingService =>
        {
            var key = GenerateRandomKey();
            
            await cachingService.SetAsync(key, new CacheData
            {
                Value = "test-value"
            });

            var result = await cachingService.GetAsync<CacheData>(key);
            
            result.Value.ShouldBe("test-value");
        });
    }

    [Fact]
    public async Task ShouldRemoveByKey()
    {
        await Run<ICachingService>(async cachingService =>
        {
            var key1 = GenerateRandomKey();
            var key2 = GenerateRandomKey();
            
            await cachingService.SetAsync(key1, "test-value");
            await cachingService.SetAsync(key2, new CacheData
            {
                Value = "test-value"
            });

            await cachingService.RemoveAsync(key1);
            
            var result1 = await cachingService.GetAsync<string>(key1);
            var result2 = await cachingService.GetAsync<CacheData>(key2);
            
            result1.ShouldBeNull();
            result2.ShouldNotBeNull();
        });
    }
    
    private string GenerateRandomKey()
    {
        return Guid.NewGuid().ToString();
    }
    
    private class CacheData
    {
        public string Value { get; set; }
    }
}