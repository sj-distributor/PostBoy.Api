using Mediator.Net.Contracts;
using PostBoy.Messages.DTO.WeChat;
using PostBoy.Messages.Responses;

namespace PostBoy.Messages.Requests.WeChat;

public class GetWorkWeChatCorpApplicationsRequest : IRequest
{
    public Guid CorpId { get; set; }
}

public class GetWorkWeChatCorpApplicationsResponse : PostBoyResponse<List<WorkWeChatCorpApplicationDto>>
{
}