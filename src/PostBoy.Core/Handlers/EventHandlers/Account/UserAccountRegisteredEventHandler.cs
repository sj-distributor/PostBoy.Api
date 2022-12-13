using Mediator.Net.Context;
using Mediator.Net.Contracts;
using PostBoy.Messages.Events.Account;

namespace PostBoy.Core.Handlers.EventHandlers.Account;

public class UserAccountRegisteredEventHandler : IEventHandler<UserAccountRegisteredEvent>
{
    public Task Handle(IReceiveContext<UserAccountRegisteredEvent> context, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}