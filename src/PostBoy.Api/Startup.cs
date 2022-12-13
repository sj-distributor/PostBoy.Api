using Serilog;
using Autofac;
using PostBoy.Core;
using PostBoy.Api.Extensions;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace PostBoy.Api;

public class Startup
{
    private IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddMvc();
        services.AddControllers().AddNewtonsoftJson();
        services.AddHttpContextAccessor();
        services.AddHttpClient();
        services.AddMemoryCache();
        services.AddResponseCaching();
        services.AddHealthChecks();
        services.AddCustomSwagger();
        services.AddCustomAuthentication(Configuration);
        services.AddEndpointsApiExplorer();
        services.AddCorsPolicy(Configuration);
        services.AddControllers();
        services.AddHangfireInternal(Configuration);
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "PostBoy.Api.xml");
                c.DocExpansion(DocExpansion.None);
            });
        }
        
        app.UseCors();
        app.UseRouting();
        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseHangfireInternal(Configuration);
        app.ScanHangfireRecurringJobs(Configuration);
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapHealthChecks("health");
        });
    }
    
    public void ConfigureContainer(ContainerBuilder builder)
    {
        builder.RegisterModule(new PostBoyModule(Log.Logger, typeof(PostBoyModule).Assembly));
    }
}