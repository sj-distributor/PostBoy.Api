using Mediator.Net.Context;
using Mediator.Net.Contracts;
using PostBoy.Messages.Events.Messages;

namespace PostBoy.Core.Handlers.EventHandlers.Messages;

public class WorkWeChatAppNotificationSentEventHandler : IEventHandler<WorkWeChatAppNotificationSentEvent>
{
    public Task Handle(IReceiveContext<WorkWeChatAppNotificationSentEvent> context, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}