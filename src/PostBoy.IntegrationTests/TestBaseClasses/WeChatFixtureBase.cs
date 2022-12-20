using Xunit;

namespace PostBoy.IntegrationTests.TestBaseClasses;

[Collection("Account Tests")]
public class WeChatFixtureBase : TestBase
{
    protected WeChatFixtureBase() : base("_wechat_", "postboy_wechat", 1)
    {
    }
}