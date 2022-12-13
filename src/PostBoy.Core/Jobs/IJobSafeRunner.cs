using Autofac;
using PostBoy.Core.Ioc;
using Serilog.Context;
using System.ComponentModel;

namespace PostBoy.Core.Jobs;

public interface IJobSafeRunner : IScopedDependency
{
    [DisplayName("{0}")]
    Task Run(string jobId, Type jobType);
}

public class JobSafeRunner : IJobSafeRunner
{
    private readonly ILifetimeScope _lifetimeScope;

    public JobSafeRunner(ILifetimeScope lifetimeScope)
    {
        _lifetimeScope = lifetimeScope;
    }

    public async Task Run(string jobId, Type jobType)
    {
        await using var newScope = _lifetimeScope.BeginLifetimeScope();
        
        var job = (IJob) newScope.Resolve(jobType);
        
        using (LogContext.PushProperty("JobId", job.JobId))
        {
            await job.Execute();
        }
    }
}