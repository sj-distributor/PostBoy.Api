using System.Net;
using System.Runtime.ExceptionServices;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.Pipeline;
using PostBoy.Messages.Responses;

namespace PostBoy.Core.Middlewares.UnifyResponse;

public class UnifyResponseSpecification<TContext> : IPipeSpecification<TContext>
    where TContext : IContext<IMessage>
{
    public bool ShouldExecute(TContext context, CancellationToken cancellationToken)
    {
        return true;
    }

    public Task BeforeExecute(TContext context, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task Execute(TContext context, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task AfterExecute(TContext context, CancellationToken cancellationToken)
    {
        EnrichResponse(context, null);
        
        return Task.CompletedTask;
    }

    public Task OnException(Exception ex, TContext context)
    {
        EnrichResponse(context, ex);
        
        ExceptionDispatchInfo.Capture(ex).Throw();
        
        throw ex;
    }

    private void EnrichResponse(TContext context, Exception? ex)
    {
        if (!ShouldExecute(context, default) || context.Result is not PostBoyResponse) return;

        var response = (dynamic)context.Result;

        if (ex == null)
        {
            response.Code = HttpStatusCode.OK;
            response.Msg = nameof(HttpStatusCode.OK).ToLower();
        }
        else
        {
            response.Code = HttpStatusCode.InternalServerError;
            response.Msg = ex.Message;
        }
    }
}