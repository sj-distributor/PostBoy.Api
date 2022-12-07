using System.Net;
using Mediator.Net.Contracts;

namespace PostBoy.Messages.Responses;

public class PostBoyResponse<T> : PostBoyResponse
{
    public T Data { get; set; }
}

public class PostBoyResponse : IResponse
{
    public string Msg { get; set; }

    public HttpStatusCode Code { get; set; }
}