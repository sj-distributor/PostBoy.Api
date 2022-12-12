using Mediator.Net.Contracts;
using PostBoy.Messages.Responses;

namespace PostBoy.Messages.Commands.Account;

public class RegisterCommand : ICommand
{
    public string UserName { get; set; }
    
    public string Password { get; set; }
}

public class RegisterResponse : PostBoyResponse<bool>
{
}