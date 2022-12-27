using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication;
using PostBoy.Core.Services.Account;

namespace PostBoy.Api.Authentication.ApiKey;

public class ApiKeyAuthenticationHandler : AuthenticationHandler<ApiKeyAuthenticationOptions>
{
    private readonly IAccountDataProvider _accountDataProvider;
    
    public ApiKeyAuthenticationHandler(IOptionsMonitor<ApiKeyAuthenticationOptions> options, ILoggerFactory logger,
        UrlEncoder encoder, ISystemClock clock, IAccountDataProvider accountDataProvider) : base(options, logger, encoder, clock)
    {
        _accountDataProvider = accountDataProvider;
    }
    
    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.ContainsKey("X-API-KEY"))
            return AuthenticateResult.NoResult();
        
        var apiKey = Context.Request.Headers["X-API-KEY"].ToString();
        
        if (string.IsNullOrWhiteSpace(apiKey))
            return AuthenticateResult.NoResult();
        
        var keyUser = await _accountDataProvider.GetUserAccountByApiKeyAsync(apiKey).ConfigureAwait(false);

        if (keyUser == null)
            return AuthenticateResult.NoResult();
        
        var identity = new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Name, keyUser.UserName),
            new Claim(ClaimTypes.NameIdentifier, keyUser.Id.ToString()),
        }, AuthenticationSchemeConstants.ApiKeyAuthenticationScheme);

        var claimsPrincipal = new ClaimsPrincipal(identity);

        var authenticationTicket = new AuthenticationTicket(claimsPrincipal,
            new AuthenticationProperties { IsPersistent = false }, Scheme.Name);
            
        Request.HttpContext.User = claimsPrincipal;

        return AuthenticateResult.Success(authenticationTicket);

    }
}