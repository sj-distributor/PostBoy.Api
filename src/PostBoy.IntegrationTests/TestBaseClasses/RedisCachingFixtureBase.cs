using Xunit;

namespace PostBoy.IntegrationTests.TestBaseClasses;

[Collection("RedisCaching Tests")]
public class RedisCachingFixtureBase : TestBase
{
    protected RedisCachingFixtureBase() : base("_authentication_", "postboy_wechat", 1)
    {
    }
}