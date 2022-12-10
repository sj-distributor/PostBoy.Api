using FluentValidation;
using PostBoy.Core.Middlewares.FluentMessageValidator;
using PostBoy.Messages.Commands.Messages;

namespace PostBoy.Core.Validators.Commands.Messages;

public class SendMessageCommandValidator : FluentMessageValidator<SendMessageCommand>
{
    public SendMessageCommandValidator()
    {
        When(x => x.WorkWeChatAppNotification != null, () =>
        {
            RuleFor(x => x.WorkWeChatAppNotification.AppId).NotEmpty();
        });
    }
}