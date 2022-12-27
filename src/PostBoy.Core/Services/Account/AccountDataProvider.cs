using System.Security.Claims;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using PostBoy.Core.Data;
using PostBoy.Core.Domain.Account;
using PostBoy.Core.Domain.Authentication;
using PostBoy.Core.Extensions;
using PostBoy.Core.Ioc;
using PostBoy.Messages.DTO.Account;
using PostBoy.Messages.DTO.Authentication;

namespace PostBoy.Core.Services.Account;

public interface IAccountDataProvider : IScopedDependency
{
    Task<(bool CanLogin, UserAccountDto Account)> AuthenticateAsync(
        string username, string clearTextPassword, CancellationToken cancellationToken);
    
    Task<UserAccountDto> GetUserAccountAsync(Guid? id = null, string username = null, bool includeRoles = false,
        CancellationToken cancellationToken = default);

    List<Claim> GenerateClaimsFromUserAccount(UserAccountDto account);
    
    Task<UserAccount> CreateUserAccount(string userName, string password, CancellationToken cancellationToken);
    
    Task<UserAccountApiKeyDto> GetUserAccountByApiKeyAsync(string apiKey, CancellationToken cancellationToken);
}

public partial class AccountDataProvider : IAccountDataProvider
{
    private readonly IMapper _mapper;
    private readonly IRepository _repository;

    public AccountDataProvider(IMapper mapper, IRepository repository)
    {
        _mapper = mapper;
        _repository = repository;
    }

    public async Task<UserAccountDto> GetUserAccountAsync(Guid? id = null, string username = null, bool includeRoles = false,
        CancellationToken cancellationToken = default)
    {
        var query = _repository.QueryNoTracking<UserAccount>();

        if (id.HasValue)
            query = query.Where(x => x.Id == id);
        
        if (!string.IsNullOrEmpty(username))
            query = query.Where(x => x.UserName == username);
        
        var account = await query
            .ProjectTo<UserAccountDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync(cancellationToken).ConfigureAwait(false);

        if (account == null || !includeRoles) return account;
        {
            var userRoleIds = await _repository
                .QueryNoTracking<RoleUser>().Where(x => x.UserId == account.Id)
                .Select(x => x.RoleId).ToListAsync(cancellationToken).ConfigureAwait(false);

            if (userRoleIds.Any())
            {
                account.Roles = await _repository
                    .QueryNoTracking<Role>()
                    .Where(x => userRoleIds.Contains(x.Id))
                    .ProjectTo<RoleDto>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken).ConfigureAwait(false);
            }
        }

        return account;
    }

    public List<Claim> GenerateClaimsFromUserAccount(UserAccountDto account)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, account.UserName),
            new(ClaimTypes.NameIdentifier, account.Id.ToString())
        };
        
        claims.AddRange(account.Roles.Select(r => new Claim(ClaimTypes.Role, r.Name)));
        
        return claims;
    }
    
    public async Task<UserAccount> CreateUserAccount(string requestUserName, string requestPassword, CancellationToken cancellationToken)
    {
        var userAccount = new UserAccount
        {
            UserName = requestUserName,
            Password = requestPassword.ToSha256(),
            IsActive = true
        };

        await _repository.InsertAsync(userAccount, cancellationToken).ConfigureAwait(false);

        return userAccount;
    }

    public async Task<UserAccountApiKeyDto> GetUserAccountByApiKeyAsync(string apiKey, CancellationToken cancellationToken)
    {
        return await _repository.QueryNoTracking<UserAccountApiKey>().Where(x => x.ApiKey == apiKey)
            .ProjectTo<UserAccountApiKeyDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync(cancellationToken).ConfigureAwait(false);
    }
}