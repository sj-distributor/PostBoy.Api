using Mediator.Net.Context;
using Mediator.Net.Contracts;
using PostBoy.Core.Services.WeChat;
using PostBoy.Messages.Requests.WeChat;

namespace PostBoy.Core.Handlers.RequestHandlers.WeChat;

public class GetWorkWeChatCorpApplicationsRequestHandler : IRequestHandler<GetWorkWeChatCorpApplicationsRequest, GetWorkWeChatCorpApplicationsResponse>
{
    private readonly IWeChatService _weChatService;

    public GetWorkWeChatCorpApplicationsRequestHandler(IWeChatService weChatService)
    {
        _weChatService = weChatService;
    }

    public async Task<GetWorkWeChatCorpApplicationsResponse> Handle(IReceiveContext<GetWorkWeChatCorpApplicationsRequest> context, CancellationToken cancellationToken)
    {
        return await _weChatService.GetWorkWeChatCorpApplicationsAsync(context.Message, cancellationToken).ConfigureAwait(false);
    }
}