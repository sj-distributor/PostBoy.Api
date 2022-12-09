using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace PostBoy.Core.Services.Identity;

public interface ICurrentUser
{
    Guid Id { get; }
}

public class CurrentUser : ICurrentUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUser(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid Id
    {
        get
        {
            if (_httpContextAccessor.HttpContext == null)
                throw new ApplicationException("HttpContext is not available");

            var idClaim = _httpContextAccessor.HttpContext.User.Claims.Single(x => x.Type == ClaimTypes.NameIdentifier);

            return Guid.Parse(idClaim.Value);
        }
    }
}