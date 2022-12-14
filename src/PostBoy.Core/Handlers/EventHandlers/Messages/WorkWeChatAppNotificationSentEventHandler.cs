using Mediator.Net.Context;
using Mediator.Net.Contracts;
using PostBoy.Messages.Events.Messages;
using Serilog;

namespace PostBoy.Core.Handlers.EventHandlers.Messages;

public class WorkWeChatAppNotificationSentEventHandler : IEventHandler<WorkWeChatAppNotificationSentEvent>
{
    public Task Handle(IReceiveContext<WorkWeChatAppNotificationSentEvent> context, CancellationToken cancellationToken)
    {
        Log.Information(
            "Send work wechat message: {@Message}, response: {@Response}, upload file responses: {@FileResponses}", 
            context.Message.SentMessage, context.Message.SentResponse, context.Message.UploadFilesDic);

        return Task.CompletedTask;
    }
}