using PostBoy.Core.Services.Identity;

namespace PostBoy.IntegrationTests;

public class TestCurrentUser : ICurrentUser
{
    public Guid Id { get; } = Guid.NewGuid();

    public string UserName { get; set; } = "TEST_USER";
}