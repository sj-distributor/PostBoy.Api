using Xunit;

namespace PostBoy.IntegrationTests.TestBaseClasses;

[Collection("Generic Tests")]
public class GenericFixtureBase : TestBase
{
    protected GenericFixtureBase() : base("_generic_", "postboy_generic", 2)
    {
    }
}