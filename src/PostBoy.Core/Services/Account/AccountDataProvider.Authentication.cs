using PostBoy.Core.Domain.Account;
using PostBoy.Core.Extensions;
using PostBoy.Messages.DTO.Account;

namespace PostBoy.Core.Services.Account;

public partial class AccountDataProvider
{
    public async Task<(bool CanLogin, UserAccountDto Account)> AuthenticateAsync(
        string username, string clearTextPassword, CancellationToken cancellationToken)
    {
        var hashPassword = clearTextPassword.ToSha256();

        var canLogin = await _repository
            .AnyAsync<UserAccount>(x => x.UserName == username && x.Password == hashPassword && x.IsActive,
                cancellationToken).ConfigureAwait(false);

        if (!canLogin) 
            return (false, null);
        
        var account = await GetUserAccountAsync(username: username, includeRoles: true,
            cancellationToken: cancellationToken).ConfigureAwait(false);
        
        return (true, account);
    }
}