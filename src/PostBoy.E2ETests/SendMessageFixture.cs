using Microsoft.Extensions.DependencyInjection;
using PostBoy.Core.Data;
using PostBoy.Core.Domain.Account;

namespace PostBoy.E2ETests;

[Collection("Sequential")]
public class SendMessageFixture : IClassFixture<ApiTestFixture>
{
    private readonly ApiTestFixture _factory;

    public SendMessageFixture(ApiTestFixture factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task ShouldSendWorkWeChatNotification()
    {
        var repo = _factory.Services.GetRequiredService<IRepository>();
        var unitOfWork = _factory.Services.GetRequiredService<IUnitOfWork>();

        await repo.InsertAsync(new Role
        {
            Name = "test"
        });
        await unitOfWork.SaveChangesAsync();
    }
}