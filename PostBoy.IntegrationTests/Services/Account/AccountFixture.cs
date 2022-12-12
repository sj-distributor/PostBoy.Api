using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Mediator.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PostBoy.Core.Data;
using PostBoy.Core.Domain.Account;
using PostBoy.Core.Extensions;
using PostBoy.Core.Settings.Authentication;
using PostBoy.IntegrationTests.TestBaseClasses;
using PostBoy.IntegrationTests.Utils.Account;
using PostBoy.Messages.Requests.Account;
using Shouldly;
using Xunit;

namespace PostBoy.IntegrationTests.Services.Account;

public class AccountFixture : AccountFixtureBase
{
    private readonly AccountUtil _accountUtil;

    public AccountFixture()
    {
        _accountUtil = new AccountUtil(CurrentScope);
    }

    [Fact]
    public void ShouldSha256Correct()
    {
        var clearText = "ece18047-239b-4309-b52d-472d9d2dfc15";
        
        clearText.ToSha256().ToUpper().ShouldBe("a3b1282868e2797613d4b647febd6b5c8b4f28db1d0d7c195765a31f0dd5f765".ToUpper());
    }

    [Theory]
    [InlineData("admin", "123456", true, true)]
    [InlineData("admin", "123456", false, false)]
    [InlineData("admin", "1234567", true, false)]
    [InlineData("admin1", "123456", true, false)]
    public async Task CanLogin(string username, string password, bool isActive, bool canLogin)
    {
        await _accountUtil.AddUserAccount("admin", "123456", isActive: isActive);

        await Run<IMediator>(async mediator =>
        {
            var response = await mediator.RequestAsync<LoginRequest, LoginResponse>(new LoginRequest
            {
                UserName = username,
                Password = password
            });

            if (canLogin)
                response.Data.ShouldNotBeEmpty();
            else
                response.Data.ShouldBeNull();
        });
    }

    [Fact]
    public async Task TokenProvideAfterLoginShouldBeValidated()
    {
        await _accountUtil.AddUserAccount("admin", "123456", isActive: true);
        
        var token = await Run<IMediator, string>(async mediator =>
        {
            var response = await mediator.RequestAsync<LoginRequest, LoginResponse>(new LoginRequest
            {
                UserName = "admin",
                Password = "123456"
            });

            return response.Data;
        });

        token.ShouldNotBeEmpty();
        
        var validationParameters = new TokenValidationParameters
        {
            ValidateLifetime = false,
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey =
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(new JwtSymmetricKeySetting(CurrentConfiguration).Value
                        .PadRight(256 / 8, '\0')))
        };
        
        new JwtSecurityTokenHandler().ValidateToken(token, validationParameters, out var validToken);

        validToken.ShouldNotBeNull();
    }

    [Fact]
    public async Task CanRegisterUserAccount()
    {
        var userName = "bans";
        var passWord = "123456";
        
        await _accountUtil.RegisterUserAccount(userName, passWord);
        
        var userAccounts = await Run<IRepository, List<UserAccount>>(async repository =>
            await repository.Query<UserAccount>().ToListAsync().ConfigureAwait(false));

        var userAccount = userAccounts.FirstOrDefault(x => x.UserName == userName);
        
        userAccount.ShouldNotBeNull();
        userAccount.UserName.ShouldBe(userName);
        userAccount.Password.ShouldBe(passWord.ToSha256());
        userAccount.IsActive.ShouldBe(true);
        
        await Run<IMediator>(async mediator =>
        {
            var response = await mediator.RequestAsync<LoginRequest, LoginResponse>(new LoginRequest
            {
                UserName = userName,
                Password = passWord
            });

            response.Data.ShouldNotBeEmpty();
        });

        await MarkUserAccountAsUnActivate(userAccount);
        
        await Run<IMediator>(async mediator =>
        {
            var response = await mediator.RequestAsync<LoginRequest, LoginResponse>(new LoginRequest
            {
                UserName = userName,
                Password = passWord
            });
        
            response.Data.ShouldBeNull();
        });
    }

    private async Task<UserAccount> MarkUserAccountAsUnActivate(UserAccount userAccount)
    {
        userAccount.IsActive = false;
        
        await RunWithUnitOfWork<IRepository>(async repository => { await repository.UpdateAsync(userAccount).ConfigureAwait(false); });
        
        return await Run<IRepository, UserAccount>(async repository =>
            await repository.GetByIdAsync<UserAccount>(userAccount.Id, CancellationToken.None).ConfigureAwait(false));
    }
}