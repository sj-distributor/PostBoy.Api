using System.Reflection;
using Autofac;
using Serilog;
using Mediator.Net;
using Mediator.Net.Autofac;
using PostBoy.Core.Ioc;
using AutoMapper.Contrib.Autofac.DependencyInjection;
using Mediator.Net.Middlewares.Serilog;
using Microsoft.EntityFrameworkCore;
using PostBoy.Core.Data;
using PostBoy.Core.Middlewares.FluentMessageValidator;
using PostBoy.Core.Middlewares.UnifyResponse;
using PostBoy.Core.Middlewares.UnitOfWork;
using PostBoy.Core.Settings;
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
        RegisterSettings(builder);
        RegisterDatabase(builder);
        RegisterDependency(builder);
        RegisterAutoMapper(builder);
        RegisterValidators(builder);
    }
    
    private void RegisterLogger(ContainerBuilder builder)
    {
        builder.RegisterInstance(_logger).AsSelf().AsImplementedInterfaces().SingleInstance();
    }
    
    private void RegisterAutoMapper(ContainerBuilder builder)
    {
        builder.RegisterAutoMapper(typeof(PostBoyModule).Assembly);
    }
    
    private void RegisterSettings(ContainerBuilder builder)
    {
        var settingTypes = typeof(PostBoyModule).Assembly.GetTypes()
            .Where(t => t.IsClass && typeof(IConfigurationSetting).IsAssignableFrom(t))
            .ToArray();

        builder.RegisterTypes(settingTypes).AsSelf().SingleInstance();
    }
    
    private void RegisterMediator(ContainerBuilder builder)
    {
        var mediatorBuilder = new MediatorBuilder();
        mediatorBuilder.RegisterHandlers(_assemblies);
        mediatorBuilder.ConfigureGlobalReceivePipe(c =>
        {
            c.UseMessageValidator();
            c.UseUnitOfWork();
            c.UseUnifyResponse();
            c.UseSerilog(logger: _logger);
        });
        builder.RegisterMediator(mediatorBuilder);
    }
    
    private void RegisterDatabase(ContainerBuilder builder)
    {
        builder.RegisterType<PostBoyDbContext>()
            .AsSelf()
            .As<DbContext>()
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();
    
        builder.RegisterType<EfRepository>().As<IRepository>().InstancePerLifetimeScope();
    }
    
    private void RegisterDependency(ContainerBuilder builder)
    {
        foreach (var type in typeof(IDependency).Assembly.GetTypes()
                     .Where(type => type.IsClass && typeof(IDependency).IsAssignableFrom(type)))
        {
            if (typeof(IScopedDependency).IsAssignableFrom(type))
                builder.RegisterType(type).AsSelf().AsImplementedInterfaces().InstancePerLifetimeScope();
            else if (typeof(ISingletonDependency).IsAssignableFrom(type))
                builder.RegisterType(type).AsSelf().AsImplementedInterfaces().SingleInstance();
            else if (typeof(ITransientDependency).IsAssignableFrom(type))
                builder.RegisterType(type).AsSelf().AsImplementedInterfaces().InstancePerDependency();
            else
                builder.RegisterType(type).AsSelf().AsImplementedInterfaces();
        }
    }
    
    private void RegisterValidators(ContainerBuilder builder)
    {
        builder.RegisterTypes(typeof(PostBoyModule).Assembly.GetTypes()
            .Where(x => x.IsClass && typeof(IFluentMessageValidator).IsAssignableFrom(x)).ToArray()).AsSelf().AsImplementedInterfaces();
    }
}