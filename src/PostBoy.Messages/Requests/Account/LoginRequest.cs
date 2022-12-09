using Mediator.Net.Contracts;
using PostBoy.Messages.Responses;

namespace PostBoy.Messages.Requests.Account;

public class LoginRequest : IRequest
{
    public string UserName { get; set; }
    
    public string Password { get; set; }
}

public class LoginResponse : PostBoyResponse<string>
{
}