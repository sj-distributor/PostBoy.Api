using Hangfire;
using PostBoy.Core.Jobs;
using PostBoy.Core.Settings.Caching;
using Serilog;
using Smarties.Core.Jobs;

namespace PostBoy.Api.Extensions;

public static class HangfireExtension
{
    public static void AddHangfireInternal(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHangfire(c =>
        {
            c.UseFilter(new AutomaticRetryAttribute { Attempts = 0 });
            c.UseRedisStorage(new RedisCacheConnectionStringSetting(configuration).Value);
        });
        
        services.AddHangfireServer();
    }

    public static void UseHangfireInternal(this IApplicationBuilder app, IConfiguration configuration)
    {
        app.UseHangfireDashboard(options: new DashboardOptions
        {
            IgnoreAntiforgeryToken = true
        });
    }
    
    public static void ScanHangfireRecurringJobs(this IApplicationBuilder app, IConfiguration configuration)
    {
        var recurringJobTypes = typeof(IRecurringJob).Assembly.GetTypes().Where(type => type.IsClass && typeof(IRecurringJob).IsAssignableFrom(type)).ToList();
        
        foreach (var type in recurringJobTypes)
        {
            var job = (IRecurringJob) app.ApplicationServices.GetRequiredService(type);

            if (string.IsNullOrEmpty(job.CronExpression))
            {
                Log.Error("Recurring Job Cron Expression Empty, {Job}", job.GetType().FullName);
                continue;
            }
            
            RecurringJob.AddOrUpdate<IJobSafeRunner>(job.JobId, r => r.Run(job.JobId, type), job.CronExpression, job.TimeZone);
        }
    }
}