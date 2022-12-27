using Mediator.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PostBoy.Messages.Commands.Messages;

namespace PostBoy.Api.Controller;

[ApiController]
[Route("api/[controller]")]
public class MessageController : ControllerBase
{
    private readonly IMediator _mediator;

    public MessageController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [Authorize(AuthenticationSchemes = "X-API-KEY")]
    [Route("send"), HttpPost]
    public async Task<IActionResult> SendMessageAsync([FromBody] SendMessageCommand command)
    {
        await _mediator.SendAsync(command).ConfigureAwait(false);

        return Ok();
    }
}