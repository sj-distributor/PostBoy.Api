using Xunit;

namespace PostBoy.IntegrationTests.TestBaseClasses;

[Collection("Account Tests")]
public class AccountFixtureBase : TestBase
{
    protected AccountFixtureBase() : base("_account_", "PostBoy_account", 0)
    {
    }
}