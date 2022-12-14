using Newtonsoft.Json;
using PostBoy.Core.Domain.WeChat;

namespace PostBoy.Core.Services.WeChat.Exceptions;

public class WorkWeChatAppNotificationCorpMissingException : Exception
{
    public WorkWeChatAppNotificationCorpMissingException(string appId) 
        : base($"Corp missing {appId}")
    {
    }
}