using Mediator.Net.Contracts;
using PostBoy.Messages.DTO.Messages;

namespace PostBoy.Messages.Commands.Messages;

public class SendMessageCommand : ICommand
{
    public SendWorkWeChatAppNotificationDto WorkWeChatAppNotification { get; set; }
}