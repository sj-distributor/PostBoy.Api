using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using PostBoy.Core.Services.Account;

namespace PostBoy.Api.Authentication;

public class ApiKeyAuthenticationHandler : AuthenticationHandler<ApiKeyAuthenticationOptions>
{
    private readonly IAccountService _service;
    
    public ApiKeyAuthenticationHandler(IOptionsMonitor<ApiKeyAuthenticationOptions> options, ILoggerFactory logger,
        UrlEncoder encoder, ISystemClock clock, IAccountService service) : base(options, logger, encoder, clock)
    {
        _service = service;
    }
    
    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.ContainsKey("X-API-KEY"))
            return AuthenticateResult.NoResult();
        
        var apiKey = Context.Request.Headers["X-API-KEY"].ToString();
        
        if (string.IsNullOrWhiteSpace(apiKey))
        {
            return AuthenticateResult.NoResult();
        }
        
        var account = await _service.GetUserAccountByApiKeyAsync(apiKey, CancellationToken.None).ConfigureAwait(false);
        
        if (account != null)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, "X-API-KEY"),
                new Claim(ClaimTypes.NameIdentifier, account.Id.ToString())
            };
            
            var identity = new ClaimsIdentity(claims, authenticationType: "X-API-KEY");
            
            var identities = new List<ClaimsIdentity> { identity };
            
            var principal = new ClaimsPrincipal(identities);
            
            var ticket = new AuthenticationTicket(principal, authenticationScheme: "X-API-KEY");

            await Task.CompletedTask;
            
            return AuthenticateResult.Success(ticket);
        }

        return AuthenticateResult.Fail("Invalid apkKey provided.");
    }
}