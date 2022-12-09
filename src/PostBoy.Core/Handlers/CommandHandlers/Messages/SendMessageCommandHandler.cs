using Mediator.Net.Context;
using Mediator.Net.Contracts;
using PostBoy.Messages.Commands.Messages;

namespace PostBoy.Core.Handlers.CommandHandlers.Messages;

public class SendMessageCommandHandler : ICommandHandler<SendMessageCommand>
{
    public Task Handle(IReceiveContext<SendMessageCommand> context, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}