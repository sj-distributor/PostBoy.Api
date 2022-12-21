using Microsoft.AspNetCore.Authentication;

namespace PostBoy.Api.Authentication;

public static class AuthenticationBuilderExtensions
{
    public static AuthenticationBuilder AddApiKeySupport(this AuthenticationBuilder authenticationBuilder)
    {
        return authenticationBuilder.AddScheme<ApiKeyAuthenticationOptions, ApiKeyAuthenticationHandler>(
            ApiKeyAuthenticationOptions.DefaultScheme,
            options => { });
    }
    public static AuthenticationBuilder AddApiKeySupport(this AuthenticationBuilder authenticationBuilder,
        Action<ApiKeyAuthenticationOptions> options)
    {
        return authenticationBuilder.AddScheme<ApiKeyAuthenticationOptions, ApiKeyAuthenticationHandler>(
            ApiKeyAuthenticationOptions.DefaultScheme,
            options);
    }
}