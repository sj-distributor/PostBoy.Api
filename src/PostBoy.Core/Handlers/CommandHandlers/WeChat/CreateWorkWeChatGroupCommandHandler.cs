using Mediator.Net.Context;
using Mediator.Net.Contracts;
using PostBoy.Core.Services.WeChat;
using PostBoy.Messages.Commands.WeChat;

namespace PostBoy.Core.Handlers.CommandHandlers.WeChat;

public class CreateWorkWeChatGroupCommandHandler : ICommandHandler<CreateWorkWeChatGroupCommand, CreateWorkWeChatGroupResponse>
{
    private readonly IWeChatService _weChatService;

    public CreateWorkWeChatGroupCommandHandler(IWeChatService weChatService)
    {
        _weChatService = weChatService;
    }

    public async Task<CreateWorkWeChatGroupResponse> Handle(IReceiveContext<CreateWorkWeChatGroupCommand> context, CancellationToken cancellationToken)
    {
        return await _weChatService.CreateWorkWeChatGroupAsync(context.Message, cancellationToken).ConfigureAwait(false);
    }
}