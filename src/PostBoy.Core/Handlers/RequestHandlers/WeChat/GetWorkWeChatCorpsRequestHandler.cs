using Mediator.Net.Context;
using Mediator.Net.Contracts;
using PostBoy.Core.Services.WeChat;
using PostBoy.Messages.Requests.WeChat;

namespace PostBoy.Core.Handlers.RequestHandlers.WeChat;

public class GetWorkWeChatCorpsRequestHandler : IRequestHandler<GetWorkWeChatCorpsRequest, GetWorkWeChatCorpsResponse>
{
    private readonly IWeChatService _weChatService;

    public GetWorkWeChatCorpsRequestHandler(IWeChatService weChatService)
    {
        _weChatService = weChatService;
    }

    public async Task<GetWorkWeChatCorpsResponse> Handle(IReceiveContext<GetWorkWeChatCorpsRequest> context, CancellationToken cancellationToken)
    {
        return await _weChatService.GetWorkWeChatCorpsAsync(context.Message, cancellationToken).ConfigureAwait(false);
    }
}