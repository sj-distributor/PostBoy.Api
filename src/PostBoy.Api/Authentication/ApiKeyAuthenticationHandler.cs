using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace PostBoy.Api.Authentication;

public class ApiKeyAuthenticationHandler : AuthenticationHandler<ApiKeyAuthenticationOptions>
{
    private readonly (string Owner, string Key)[] _apiKeys = new[] { ("admin", "123") };
    private readonly ApiKeyAuthenticationOptions _options;

    public ApiKeyAuthenticationHandler(IOptionsMonitor<ApiKeyAuthenticationOptions> options, ILoggerFactory logger,
        UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
    {
        _options = options.CurrentValue;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (Request.HttpContext.User.Identity == null || Request.HttpContext.User.Identity.IsAuthenticated)
            return AuthenticateResult.NoResult();
            
        if (!Request.Headers.ContainsKey("X-API-KEY"))
            return AuthenticateResult.NoResult();
        
        var providedApiKey = Context.Request.Headers["X-API-KEY"].ToString();
        
        if (string.IsNullOrWhiteSpace(providedApiKey))
        {
            return AuthenticateResult.NoResult();
        }

        var apiKey = _apiKeys.FirstOrDefault(k => k.Key == providedApiKey);
        
        if (apiKey != default)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, apiKey.Owner)
            };
            var identity = new ClaimsIdentity(claims, authenticationType: _options.AuthenticationType);
            
            var identities = new List<ClaimsIdentity> { identity };
            
            var principal = new ClaimsPrincipal(identities);
            
            var ticket = new AuthenticationTicket(principal, authenticationScheme: _options.Scheme);

            await Task.CompletedTask;
            
            return AuthenticateResult.Success(ticket);
        }

        return AuthenticateResult.Fail("Invalid X-API-KEY provided.");
    }
}