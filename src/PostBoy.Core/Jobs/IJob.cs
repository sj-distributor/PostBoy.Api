using PostBoy.Core.Ioc;

namespace PostBoy.Core.Jobs;

public interface IJob : IScopedDependency
{
    Task Execute();
    
    string JobId { get; }
}