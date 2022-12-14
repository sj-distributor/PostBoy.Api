using Mediator.Net.Context;
using Mediator.Net.Contracts;
using PostBoy.Core.Services.Messages;
using PostBoy.Messages.Commands.Messages;

namespace PostBoy.Core.Handlers.CommandHandlers.Messages;

public class SendWorkWeChatAppNotificationCommandHandler : ICommandHandler<SendWorkWeChatAppNotificationCommand>
{
    private readonly IMessageService _messageService;

    public SendWorkWeChatAppNotificationCommandHandler(IMessageService messageService)
    {
        _messageService = messageService;
    }

    public async Task Handle(IReceiveContext<SendWorkWeChatAppNotificationCommand> context, CancellationToken cancellationToken)
    {
        var @event = await _messageService.SendMessageAsync(context.Message, cancellationToken).ConfigureAwait(false);

        await context.PublishAsync(@event, cancellationToken).ConfigureAwait(false);
    }
}