using AutoMapper;
using PostBoy.Core.Domain.WeChat;
using PostBoy.Messages.Commands.WeChat;
using PostBoy.Messages.DTO.WeChat;

namespace PostBoy.Core.Mappings;

public class WeChatMapping : Profile
{
    public WeChatMapping()
    {
        CreateMap<CreateWorkWeChatGroupCommand, CreateWorkWeChatGroupDto>();
        CreateMap<WorkWeChatCorp, WorkWeChatCorpDto>();
        CreateMap<WorkWeChatCorpApplication, WorkWeChatCorpApplicationDto>();
    }
}