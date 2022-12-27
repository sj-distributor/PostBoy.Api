using System.Net;
using PostBoy.Core.Domain.Authentication;
using PostBoy.Core.Ioc;
using PostBoy.Messages.Commands.Account;
using PostBoy.Messages.DTO.Authentication;
using PostBoy.Messages.Events.Account;
using PostBoy.Messages.Requests.Account;

namespace PostBoy.Core.Services.Account;

public interface IAccountService : IScopedDependency
{
    Task<LoginResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken);
    
    Task<UserAccountRegisteredEvent> RegisterAsync(RegisterCommand command, CancellationToken cancellationToken);
    
    Task<UserAccountApiKeyDto> GetUserAccountByApiKeyAsync(string apiKey, CancellationToken cancellationToken);
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
    
    public async Task<UserAccountRegisteredEvent> RegisterAsync(RegisterCommand command, CancellationToken cancellationToken)
    {
        await _accountDataProvider.CreateUserAccount(command.UserName, command.Password, cancellationToken).ConfigureAwait(false);

        return new UserAccountRegisteredEvent();
    }

    public async Task<UserAccountApiKeyDto> GetUserAccountByApiKeyAsync(string apiKey,
        CancellationToken cancellationToken)
    {
        return await _accountDataProvider.GetUserAccountByApiKeyAsync(apiKey, cancellationToken);
    }
}