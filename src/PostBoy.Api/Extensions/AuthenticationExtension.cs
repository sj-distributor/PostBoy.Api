using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using PostBoy.Api.Authentication;
using PostBoy.Api.Authentication.ApiKey;
using PostBoy.Core.Services.Identity;
using PostBoy.Core.Settings.Authentication;

namespace PostBoy.Api.Extensions;

public static class AuthenticationExtension
{
     public static void AddCustomAuthentication(this IServiceCollection services,
        IConfiguration configuration)
     {
         services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
             .AddJwtBearer(options =>
             {
                 options.TokenValidationParameters = new TokenValidationParameters
                 {
                     ValidateLifetime = false,
                     ValidateAudience = false,
                     ValidateIssuer = false,
                     ValidateIssuerSigningKey = true,
                     IssuerSigningKey =
                         new SymmetricSecurityKey(
                             Encoding.UTF8.GetBytes(new JwtSymmetricKeySetting(configuration).Value
                                 .PadRight(256 / 8, '\0')))
                 };
             })
             .AddScheme<ApiKeyAuthenticationOptions, ApiKeyAuthenticationHandler>(
                 AuthenticationSchemeConstants.ApiKeyAuthenticationScheme, _ => { });

        services.AddAuthorization(options =>
        {
            options.DefaultPolicy = new AuthorizationPolicyBuilder(
                JwtBearerDefaults.AuthenticationScheme, 
                AuthenticationSchemeConstants.ApiKeyAuthenticationScheme).RequireAuthenticatedUser().Build();
        });

        services.AddScoped<ICurrentUser, CurrentUser>();
    }
}