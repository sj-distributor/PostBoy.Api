using Mediator.Net;
using Microsoft.AspNetCore.Mvc;
using PostBoy.Messages.Commands.WeChat;

namespace PostBoy.Api.Controller;

[ApiController]
[Route("api/[controller]")]
public class WeChatController : ControllerBase
{
    private readonly IMediator _mediator;

    public WeChatController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [Route("work/group/create"), HttpPost]
    public async Task<IActionResult> CreateWorkWeChatGroupAsync([FromBody] CreateWorkWeChatGroupCommand command)
    {
        var response = await _mediator.SendAsync<CreateWorkWeChatGroupCommand, CreateWorkWeChatGroupResponse>(command).ConfigureAwait(false);

        return Ok(response.Data);
    }
}