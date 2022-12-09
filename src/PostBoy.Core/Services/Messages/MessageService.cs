using PostBoy.Core.Ioc;
using PostBoy.Core.Services.WeChat;
using PostBoy.Messages.Commands.Messages;
using PostBoy.Messages.Events.Messages;

namespace PostBoy.Core.Services.Messages;

public interface IMessageService : IScopedDependency
{
    Task<MessageSentEvent> SendMessageAsync(SendMessageCommand command, CancellationToken cancellationToken);
}

public class MessageService : IMessageService
{
    private readonly IWeChatService _weChatService;

    public MessageService(IWeChatService weChatService)
    {
        _weChatService = weChatService;
    }

    public async Task<MessageSentEvent> SendMessageAsync(SendMessageCommand command, CancellationToken cancellationToken)
    {
        await _weChatService.SendWorkWeChatAppNotificationAsync(command.WorkWeChatAppNotification, cancellationToken).ConfigureAwait(false);

        return new MessageSentEvent();
    }
}