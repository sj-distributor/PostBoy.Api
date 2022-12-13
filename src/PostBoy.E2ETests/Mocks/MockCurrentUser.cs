using PostBoy.Core.Services.Identity;

namespace PostBoy.E2ETests.Mocks;

public class MockCurrentUser : ICurrentUser
{ 
    public Guid Id => Guid.Empty;
}