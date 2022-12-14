using PostBoy.Core.Ioc;
using PostBoy.Core.Services.WeChat;
using PostBoy.Messages.Commands.Messages;
using PostBoy.Messages.Events.Messages;

namespace PostBoy.Core.Services.Messages;

public interface IMessageService : IScopedDependency
{
    Task<WorkWeChatAppNotificationSentEvent> SendMessageAsync(SendWorkWeChatAppNotificationCommand command, CancellationToken cancellationToken);
}

public partial class MessageService : IMessageService
{
    private readonly IWeChatUtilService _weChatUtilService;
    private readonly IWeChatDataProvider _weChatDataProvider;
    
    public MessageService(IWeChatUtilService weChatUtilService, IWeChatDataProvider weChatDataProvider)
    {
        _weChatUtilService = weChatUtilService;
        _weChatDataProvider = weChatDataProvider;
    }
}