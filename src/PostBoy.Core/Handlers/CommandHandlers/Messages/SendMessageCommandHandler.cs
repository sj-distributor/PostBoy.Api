using Mediator.Net.Context;
using Mediator.Net.Contracts;
using PostBoy.Core.Services.Messages;
using PostBoy.Messages.Commands.Messages;

namespace PostBoy.Core.Handlers.CommandHandlers.Messages;

public class SendMessageCommandHandler : ICommandHandler<SendMessageCommand>
{
    private readonly IMessageService _messageService;

    public SendMessageCommandHandler(IMessageService messageService)
    {
        _messageService = messageService;
    }

    public async Task Handle(IReceiveContext<SendMessageCommand> context, CancellationToken cancellationToken)
    {
        var messageSentEvent = await _messageService
            .SendMessageAsync(context.Message, cancellationToken).ConfigureAwait(false);

        await context.PublishAsync(messageSentEvent, cancellationToken).ConfigureAwait(false);
    }
}