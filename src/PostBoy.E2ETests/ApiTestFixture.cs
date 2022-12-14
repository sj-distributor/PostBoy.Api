using System.Linq.Expressions;
using Autofac;
using Mediator.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NSubstitute;
using PostBoy.Core.Services.Identity;
using PostBoy.Api;
using PostBoy.Core.Data;
using PostBoy.Core.Services.Jobs;
using PostBoy.E2ETests.Mocks;

namespace PostBoy.E2ETests;

public class ApiTestFixture : WebApplicationFactory<Startup>
{
    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureContainer<ContainerBuilder>(b =>
        {
            RegisterDatabase(b);
            RegisterCurrentUser(b);
            RegisterBackgroundJobClient(b);
        });
        return base.CreateHost(builder);
    }

    public override async ValueTask DisposeAsync()
    {
        await ClearDatabaseRecord();
        
        await base.DisposeAsync();
    }
    
    private async Task ClearDatabaseRecord()
    {
        await Services.GetRequiredService<InMemoryDbContext>().Database.EnsureDeletedAsync();
    }

    private void RegisterCurrentUser(ContainerBuilder containerBuilder)
    {
        containerBuilder.RegisterInstance(new MockCurrentUser()).As<ICurrentUser>();
    }

    private void RegisterDatabase(ContainerBuilder containerBuilder)
    {
        containerBuilder.RegisterType<InMemoryDbContext>()
            .AsSelf()
            .As<DbContext>()
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();
    
        containerBuilder.RegisterType<InMemoryRepository>().As<IRepository>().InstancePerLifetimeScope();
    }
    
    private void RegisterBackgroundJobClient(ContainerBuilder containerBuilder)
    {
        var backgroundJobClient = Substitute.For<IPostBoyBackgroundJobClient>();
        containerBuilder.Register(c =>
        {
            var instance = c.Resolve<IMediator>();
            backgroundJobClient.Enqueue(Arg.Any<Expression<Func<Task>>>()).Returns( x =>
            {
                var call = (Expression<Func<Task>>)x.Args()[0];
                var func = call.Compile();
                func().Wait();
                return string.Empty;
            });
            backgroundJobClient.Enqueue(Arg.Any<Expression<Func<IMediator, Task>>>()).Returns(x =>
            {
                var call = (Expression<Func<IMediator, Task>>)x.Args()[0];
                var func = call.Compile();
                func(instance).Wait();
                return string.Empty;
            });
            return backgroundJobClient;
        }).AsSelf().AsImplementedInterfaces();
    }
}