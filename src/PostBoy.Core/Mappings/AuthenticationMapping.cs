using AutoMapper;
using PostBoy.Core.Domain.Authentication;
using PostBoy.Messages.DTO.Authentication;

namespace PostBoy.Core.Mappings;

public class AuthenticationMapping : Profile
{
    public AuthenticationMapping()
    {
        CreateMap<UserAccountApiKey, UserAccountApiKeyDto>();
    }
}

   