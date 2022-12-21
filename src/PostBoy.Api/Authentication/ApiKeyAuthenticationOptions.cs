using Microsoft.AspNetCore.Authentication;

namespace PostBoy.Api.Authentication;

public class ApiKeyAuthenticationOptions : AuthenticationSchemeOptions
{
    public const string DefaultScheme = "X-API-KEY";
    public string Scheme { get; set; } = DefaultScheme;
    public string AuthenticationType { get; set; } = DefaultScheme;
}