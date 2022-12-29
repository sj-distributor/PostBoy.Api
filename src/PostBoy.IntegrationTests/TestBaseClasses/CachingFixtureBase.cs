using Xunit;

namespace PostBoy.IntegrationTests.TestBaseClasses;

[Collection("Caching Tests")]
public class CachingFixtureBase : TestBase
{
    protected CachingFixtureBase() : base("_caching_", "postboy_caching", 1)
    {
    }
}