using Mediator.Net.Contracts;
using PostBoy.Messages.DTO.WeChat;
using PostBoy.Messages.Responses;

namespace PostBoy.Messages.Requests.WeChat;

public class GetWorkWeChatCorpsRequest : IRequest
{
    
}

public class GetWorkWeChatCorpsResponse : PostBoyResponse<List<WorkWeChatCorpDto>>
{
}