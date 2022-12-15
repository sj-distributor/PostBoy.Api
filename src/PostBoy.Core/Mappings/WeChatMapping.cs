using AutoMapper;
using PostBoy.Messages.Commands.WeChat;
using PostBoy.Messages.DTO.WeChat;

namespace PostBoy.Core.Mappings;

public class WeChatMapping : Profile
{
    public WeChatMapping()
    {
        CreateMap<CreateWorkWeChatGroupCommand, CreateWorkWeChatGroupDto>();
    }
}