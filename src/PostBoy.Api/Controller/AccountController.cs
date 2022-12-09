using Mediator.Net;
using Microsoft.AspNetCore.Mvc;
using PostBoy.Messages.Requests.Account;

namespace PostBoy.Api.Controller;

[ApiController]
[Route("auth")]
public class AccountController : ControllerBase
{
    private readonly IMediator _mediator;

    public AccountController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [Route("login"), HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoginResponse))]
    public async Task<IActionResult> LoginAsync([FromBody] LoginRequest request)
    {
        var response = await _mediator.RequestAsync<LoginRequest, LoginResponse>(request);
        
        return Ok(response);
    }
}