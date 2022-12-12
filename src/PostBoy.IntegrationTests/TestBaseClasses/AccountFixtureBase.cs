using Xunit;

namespace PostBoy.IntegrationTests.TestBaseClasses;

[Collection("Account Tests")]
public class AccountFixtureBase : TestBase
{
    protected AccountFixtureBase() : base("_account_", "postboy_account", 0)
    {
    }
}