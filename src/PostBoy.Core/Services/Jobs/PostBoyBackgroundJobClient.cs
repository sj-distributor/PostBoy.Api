using Hangfire;
using Hangfire.States;
using PostBoy.Core.Ioc;
using System.Linq.Expressions;

namespace PostBoy.Core.Services.Jobs;

public interface IPostBoyBackgroundJobClient : IScopedDependency
{
    string Enqueue(Expression<Func<Task>> methodCall);
    
    string Enqueue<T>(Expression<Func<T, Task>> methodCall);

    string Schedule(Expression<Func<Task>> methodCall, TimeSpan delay);
    
    string Schedule(Expression<Func<Task>> methodCall, DateTimeOffset enqueueAt);
    
    string ContinueJobWith(string parentJobId, Expression<Func<Task>> methodCall);
        
    string ContinueJobWith<T>(string parentJobId, Expression<Func<T,Task>> methodCall);
}

public class PostBoyBackgroundJobClient : IPostBoyBackgroundJobClient
{
    private readonly EnqueuedState _queue;
    private readonly Func<IBackgroundJobClient> _backgroundJobClientFunc;

    public PostBoyBackgroundJobClient(Func<IBackgroundJobClient> backgroundJobClientFunc)
    {
        _backgroundJobClientFunc = backgroundJobClientFunc;

        _queue = new EnqueuedState("default");
    }

    public string Enqueue(Expression<Func<Task>> methodCall)
    {
        return _backgroundJobClientFunc()?.Create(methodCall, _queue);
    }

    public string Enqueue<T>(Expression<Func<T, Task>> methodCall)
    {
        return _backgroundJobClientFunc()?.Create(methodCall, _queue);
    }

    public string Schedule(Expression<Func<Task>> methodCall, TimeSpan delay)
    {
        return _backgroundJobClientFunc()?.Schedule(methodCall, delay);
    }
    
    public string Schedule(Expression<Func<Task>> methodCall, DateTimeOffset enqueueAt)
    {
        return _backgroundJobClientFunc()?.Schedule(methodCall, enqueueAt);
    }

    public string ContinueJobWith(string parentJobId, Expression<Func<Task>> methodCall)
    {
        return _backgroundJobClientFunc()?.ContinueJobWith(parentJobId, methodCall, _queue);
    }
        
    public string ContinueJobWith<T>(string parentJobId, Expression<Func<T, Task>> methodCall)
    {
        return _backgroundJobClientFunc()?.ContinueJobWith(parentJobId, methodCall, _queue);
    }
}