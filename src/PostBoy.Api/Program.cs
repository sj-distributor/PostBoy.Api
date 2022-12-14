using Autofac;
using Serilog;
using Autofac.Extensions.DependencyInjection;
using PostBoy.Core;
using PostBoy.Core.Dbup;
using PostBoy.Core.Settings.Logging;
using PostBoy.Core.Settings.System;

namespace PostBoy.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddEnvironmentVariables()
            .Build();

        var apikey = new SerilogApiKeySetting(configuration).Value;
        var serverUrl = new SerilogServerUrlSetting(configuration).Value;
        var application = new SerilogApplicationSetting(configuration).Value;
        var releaseVersion = new ReleaseVersionSetting(configuration).Value;
        
        Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .Enrich.WithProperty("Application", application)
            .Enrich.WithProperty("ReleaseVersion", releaseVersion)
            .Enrich.WithCorrelationIdHeader()
            .WriteTo.Console()
            .WriteTo.Seq(serverUrl, apiKey: apikey)
            .CreateLogger();

        try
        {
            Log.Information("Configuring api host ({ApplicationContext})... ", application);

            new DbUpRunner(new PostBoyConnectionString(configuration).Value).Run();

            var webHost = CreateHostBuilder(args).Build();

            Log.Information("Starting api host ({ApplicationContext})...", application);

            webHost.Run();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Program terminated unexpectedly ({ApplicationContext})!", application);
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    private static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureLogging(l => l.AddSerilog(Log.Logger))
            .UseServiceProviderFactory(new AutofacServiceProviderFactory())
            .ConfigureContainer<ContainerBuilder>(builder =>
            {
                builder.RegisterModule(new PostBoyModule(Log.Logger, typeof(PostBoyModule).Assembly));
            })
            .ConfigureWebHostDefaults(builder => { builder.UseStartup<Startup>(); });
}