using Mediator.Net.Context;
using Mediator.Net.Contracts;
using PostBoy.Messages.Events.Messages;

namespace PostBoy.Core.Handlers.EventHandlers.Messages;

public class MessageSentEventHandler : IEventHandler<MessageSentEvent>
{
    public Task Handle(IReceiveContext<MessageSentEvent> context, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}