using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace PostBoy.Core.Services.Identity;

public interface ICurrentUser
{
    int Id { get; }
}

public class CurrentUser : ICurrentUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUser(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public int Id
    {
        get
        {
            if (_httpContextAccessor.HttpContext == null)
                throw new ApplicationException("HttpContext is not available");

            var idClaim = _httpContextAccessor.HttpContext.User.Claims.Single(x => x.Type == ClaimTypes.NameIdentifier);

            return int.Parse(idClaim.Value);
        }
    }
}

public class InternalUser : ICurrentUser
{
    public int Id => 1;
}