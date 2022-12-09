using Mediator.Net.Context;
using Mediator.Net.Contracts;
using PostBoy.Core.Services.Account;
using PostBoy.Messages.Requests.Account;

namespace PostBoy.Core.Handlers.RequestHandlers.Account;

public class LoginRequestHandler : IRequestHandler<LoginRequest, LoginResponse>
{
    private readonly IAccountService _accountService;

    public LoginRequestHandler(IAccountService accountService)
    {
        _accountService = accountService;
    }

    public async Task<LoginResponse> Handle(IReceiveContext<LoginRequest> context, CancellationToken cancellationToken)
    {
        return await _accountService.LoginAsync(context.Message, cancellationToken).ConfigureAwait(false);
    }
}