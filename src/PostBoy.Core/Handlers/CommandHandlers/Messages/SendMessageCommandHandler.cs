using Mediator.Net.Context;
using Mediator.Net.Contracts;
using PostBoy.Core.Services.WeChat;
using PostBoy.Messages.Commands.Messages;

namespace PostBoy.Core.Handlers.CommandHandlers.Messages;

public class SendMessageCommandHandler : ICommandHandler<SendMessageCommand>
{
    private readonly IWeChatService _weChatService;

    public SendMessageCommandHandler(IWeChatService weChatService)
    {
        _weChatService = weChatService;
    }

    public async Task Handle(IReceiveContext<SendMessageCommand> context, CancellationToken cancellationToken)
    {
        await _weChatService
            .SendWorkWeChatAppNotificationAsync(context.Message.WorkWeChatAppNotification, cancellationToken).ConfigureAwait(false);
    }
}