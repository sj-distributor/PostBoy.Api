using PostBoy.Core.Jobs;

namespace Smarties.Core.Jobs;

public interface IRecurringJob : IJob
{
    string CronExpression { get; }

    TimeZoneInfo TimeZone => null;
}