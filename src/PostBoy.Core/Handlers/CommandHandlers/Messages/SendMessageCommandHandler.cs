using Mediator.Net;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using PostBoy.Core.Services.Jobs;
using PostBoy.Messages.Commands.Messages;

namespace PostBoy.Core.Handlers.CommandHandlers.Messages;

public class SendMessageCommandHandler : ICommandHandler<SendMessageCommand>
{
    private readonly IPostBoyBackgroundJobClient _backgroundJobClient;

    public SendMessageCommandHandler(IPostBoyBackgroundJobClient backgroundJobClient)
    {
        _backgroundJobClient = backgroundJobClient;
    }

    public Task Handle(IReceiveContext<SendMessageCommand> context, CancellationToken cancellationToken)
    {
        _backgroundJobClient.Enqueue<IMediator>(m => m.SendAsync(new SendWorkWeChatAppNotificationCommand
        {
            WorkWeChatAppNotification = context.Message.WorkWeChatAppNotification
        }, cancellationToken));
        
        return Task.CompletedTask;
    }
}