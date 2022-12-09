using AutoMapper;
using PostBoy.Core.Domain.Account;
using PostBoy.Messages.DTO.Account;

namespace PostBoy.Core.Mappings;

public class AccountMapping : Profile
{
    public AccountMapping()
    {
        CreateMap<Role, RoleDto>();
        CreateMap<UserAccount, UserAccountDto>();
    }
}
