using Autofac;
using Mediator.Net;
using PostBoy.Core.Services.Identity;
using PostBoy.Messages.Commands.Account;

namespace PostBoy.IntegrationTests.Utils.Account;

public class IdentityUtil : TestUtil
{
    public IdentityUtil(ILifetimeScope scope) : base(scope)
    {
    }

    public async Task CreateUser(TestCurrentUser testUser)
    {
        await Run<IMediator>(async mediator =>
        {
            await mediator.SendAsync(new RegisterCommand
            {
                UserName = testUser.UserName,
                Password = "123456"
            });
        });
    }

    public void SwitchUser(ContainerBuilder builder, TestCurrentUser signUser)
    {
        builder.RegisterInstance(signUser).As<ICurrentUser>();
    }
}