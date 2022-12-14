using Mediator.Net.Contracts;
using PostBoy.Messages.DTO.WeChat;

namespace PostBoy.Messages.Events.Messages;

public class WorkWeChatAppNotificationSentEvent : IEvent
{
    public WorkWeChatSendMessageDto SentMessage { get; set; }
    
    public WorkWeChatSendMessageResponseDto SentResponse { get; set; }
    
    public Dictionary<Guid, UploadWorkWechatFileResponseDto> UploadFilesDic { get; set; }
}