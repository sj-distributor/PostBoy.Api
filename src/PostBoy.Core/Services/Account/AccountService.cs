using System.Net;
using PostBoy.Core.Ioc;
using PostBoy.Messages.Requests.Account;

namespace PostBoy.Core.Services.Account;

public interface IAccountService : IScopedDependency
{
    Task<LoginResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken);
}

public class AccountService : IAccountService
{
    private readonly ITokenProvider _tokenProvider;
    private readonly IAccountDataProvider _accountDataProvider;

    public AccountService(ITokenProvider tokenProvider, IAccountDataProvider accountDataProvider)
    {
        _tokenProvider = tokenProvider;
        _accountDataProvider = accountDataProvider;
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken)
    {
        var (canLogin, account) = await _accountDataProvider
            .AuthenticateAsync(request.UserName, request.Password, cancellationToken).ConfigureAwait(false);

        if (!canLogin)
            return new LoginResponse { Code = HttpStatusCode.Unauthorized };
        
        return new LoginResponse
        {
            Data = _tokenProvider.Generate(_accountDataProvider.GenerateClaimsFromUserAccount(account))
        };
    }
}