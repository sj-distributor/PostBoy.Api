using Mediator.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PostBoy.Messages.Commands.WeChat;
using PostBoy.Messages.Requests.WeChat;

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

        return Ok(response);
    }
    
    [Authorize]
    [Route("work/corps"), HttpGet]
    public async Task<IActionResult> GetWorkWeChatCorpsAsync()
    {
        var response = await _mediator
            .RequestAsync<GetWorkWeChatCorpsRequest, GetWorkWeChatCorpsResponse>(new GetWorkWeChatCorpsRequest()).ConfigureAwait(false);

        return Ok(response);
    }
    
    [Authorize]
    [Route("work/corp/apps"), HttpGet]
    public async Task<IActionResult> GetWorkWeChatCorpApplicationsAsync([FromQuery] GetWorkWeChatCorpApplicationsRequest request)
    {
        var response = await _mediator
            .RequestAsync<GetWorkWeChatCorpApplicationsRequest, GetWorkWeChatCorpApplicationsResponse>(request).ConfigureAwait(false);

        return Ok(response);
    }
    
    [Route("apiKeyTest"), HttpGet]
    [Authorize(AuthenticationSchemes = "apiKey")]
    public IActionResult ApiKeyAuthTest()
    {
        return Ok();
    }
}