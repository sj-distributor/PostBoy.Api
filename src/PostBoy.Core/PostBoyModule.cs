using System.Reflection;
using Autofac;
using Mediator.Net;
using Mediator.Net.Autofac;
using Serilog;
using Module = Autofac.Module;

namespace PostBoy.Core;

public class PostBoyModule : Module
{
    private readonly ILogger _logger;
    private readonly Assembly[] _assemblies;

    public PostBoyModule(ILogger logger, params Assembly[] assemblies)
    {
        _logger = logger;
        _assemblies = assemblies;

        if (_logger == null)
            throw new ArgumentException(nameof(_logger));
        
        if (_assemblies == null || !_assemblies.Any())
            throw new ArgumentException("No assemblies found to scan. Supply at least one assembly to scan.");
    }
    
    protected override void Load(ContainerBuilder builder)
    {
        RegisterLogger(builder);
        RegisterMediator(builder);
    }
    
    private void RegisterLogger(ContainerBuilder builder)
    {
        builder.RegisterInstance(_logger).AsSelf().AsImplementedInterfaces().SingleInstance();
    }
    
    private void RegisterMediator(ContainerBuilder builder)
    {
        var mediatorBuilder = new MediatorBuilder();
        mediatorBuilder.RegisterHandlers(_assemblies);
        mediatorBuilder.ConfigureGlobalReceivePipe(c =>
        {
        });
        builder.RegisterMediator(mediatorBuilder);
    }
}