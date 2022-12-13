using System.Linq.Expressions;
using Autofac;
using Mediator.Net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MySql.Data.MySqlClient;
using NSubstitute;
using PostBoy.Core.Services.Identity;
using PostBoy.Core.Settings.System;
using PostBoy.Api;
using PostBoy.Core.Services.Jobs;

namespace PostBoy.E2ETests;

public class ApiTestFixture : WebApplicationFactory<Startup>
{
    private readonly List<string> _tableRecordsDeletionExcludeList = new()
    {
        "schemaversions"
    };

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureContainer<ContainerBuilder>(b =>
        {
            RegisterCurrentUser(b);
            RegisterBackgroundJobClient(b);
        });
        return base.CreateHost(builder);
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
    }
    
    public override ValueTask DisposeAsync()
    {
        ClearDatabaseRecord();
        
        return base.DisposeAsync();
    }
    
    private void ClearDatabaseRecord()
    {
        var conn = Services.GetRequiredService<PostBoyConnectionString>();
        
        try
        {
            var connection = new MySqlConnection(conn.Value);

            var deleteStatements = new List<string>();

            connection.Open();

            using var reader = new MySqlCommand(
                    $"SELECT table_name FROM INFORMATION_SCHEMA.tables WHERE table_schema = 'postboy';",
                    connection)
                .ExecuteReader();

            deleteStatements.Add($"SET SQL_SAFE_UPDATES = 0");
            
            while (reader.Read())
            {
                var table = reader.GetString(0);

                if (!_tableRecordsDeletionExcludeList.Contains(table))
                {
                    deleteStatements.Add($"DELETE FROM `{table}`");
                }
            }

            deleteStatements.Add($"SET SQL_SAFE_UPDATES = 1");

            reader.Close();

            var strDeleteStatements = string.Join(";", deleteStatements) + ";";

            new MySqlCommand(strDeleteStatements, connection).ExecuteNonQuery();

            connection.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error cleaning up data, {ex}");
        }
    }

    private void RegisterCurrentUser(ContainerBuilder containerBuilder)
    {
        containerBuilder.RegisterInstance(new TestCurrentUser()).As<ICurrentUser>();
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

internal class TestCurrentUser : ICurrentUser
{
    public Guid Id => Guid.Empty;
}