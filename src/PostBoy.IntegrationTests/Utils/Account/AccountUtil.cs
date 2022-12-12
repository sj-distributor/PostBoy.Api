using Autofac;
using Mediator.Net;
using PostBoy.Core.Data;
using PostBoy.Core.Domain.Account;
using PostBoy.Core.Extensions;
using PostBoy.Messages.Commands.Account;

namespace PostBoy.IntegrationTests.Utils.Account;

public class AccountUtil : TestUtil
{
    public AccountUtil(ILifetimeScope scope) : base(scope)
    {
    }

    public async Task<UserAccount> AddUserAccount(string userName, string password, bool isActive = true)
    {
        return await RunWithUnitOfWork<IRepository, UserAccount>(async repository =>
        {
            var account = new UserAccount
            {
                UserName = userName,
                Password = password.ToSha256(),
                IsActive = isActive
            };
            await repository.InsertAsync(account);
            return account;
        });
    }

    public async Task RegisterUserAccount(string userName, string passWord)
    {
        await Run<IMediator>(async mediator =>
        {
            await mediator.SendAsync<RegisterCommand, RegisterResponse>(new RegisterCommand
            {
                UserName = userName,
                Password = passWord
            });
        });
    }
}