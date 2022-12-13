using Autofac;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using NSubstitute;
using PostBoy.Core;
using PostBoy.Core.Dbup;
using PostBoy.Core.Services.Identity;
using PostBoy.Core.Settings.Caching;
using PostBoy.Core.Settings.System;
using Serilog;
using StackExchange.Redis;

namespace PostBoy.IntegrationTests;

public partial class TestBase
{
    private readonly List<string> _tableRecordsDeletionExcludeList = new()
    {
        "schemaversions"
    };

    public async Task InitializeAsync()
    {
        await _identityUtil.CreateUser(new TestCurrentUser());
    }

    private void RegisterBaseContainer(ContainerBuilder containerBuilder)
    {
        var logger = Substitute.For<ILogger>();
        
        containerBuilder.RegisterModule(
            new PostBoyModule(logger, typeof(PostBoyModule).Assembly, typeof(TestBase).Assembly));

        containerBuilder.RegisterInstance(new TestCurrentUser()).As<ICurrentUser>();
        containerBuilder.RegisterInstance(Substitute.For<IMemoryCache>()).AsImplementedInterfaces();
        containerBuilder.RegisterInstance(Substitute.For<IHttpContextAccessor>()).AsImplementedInterfaces();

        RegisterConfiguration(containerBuilder);
        RegisterRedis(containerBuilder);
    }

    private void RegisterConfiguration(ContainerBuilder containerBuilder)
    {
        var targetJson = $"appsettings{_testTopic}.json";
        File.Copy("appsettings.json", targetJson, true);
        dynamic jsonObj = JsonConvert.DeserializeObject(File.ReadAllText(targetJson));
        jsonObj["ConnectionStrings"]["PostBoyConnectionString"] =
            jsonObj["ConnectionStrings"]["PostBoyConnectionString"].ToString()
                .Replace("Database=postboy", $"Database={_databaseName}");
        File.WriteAllText(targetJson, JsonConvert.SerializeObject(jsonObj));
        var configuration = new ConfigurationBuilder().AddJsonFile(targetJson).Build();
        containerBuilder.RegisterInstance(configuration).AsImplementedInterfaces();
    }

    private void RunDbUpIfRequired()
    {
        if (!TestBase.ShouldRunDbUpDatabases.GetValueOrDefault(_databaseName, true))
            return;

        new DbUpRunner(new PostBoyConnectionString(CurrentConfiguration).Value).Run();

        TestBase.ShouldRunDbUpDatabases[_databaseName] = false;
    }
    
    private void RegisterRedis(ContainerBuilder builder)
    {
        builder.Register(cfx =>
        {
            if (RedisPool.ContainsKey(_redisDatabaseIndex))
                return RedisPool[_redisDatabaseIndex];
                
            var redisConnectionSetting = cfx.Resolve<RedisCacheConnectionStringSetting>();
                
            var connString = $"{redisConnectionSetting.Value},defaultDatabase={_redisDatabaseIndex}";

            var instance = ConnectionMultiplexer.Connect(connString);
            
            return RedisPool.GetOrAdd(_redisDatabaseIndex, instance);
            
        }).ExternallyOwned();
    }
    
    private void FlushRedisDatabase()
    {
        try
        {
            if (!TestBase.RedisPool.TryGetValue(_redisDatabaseIndex, out var redis)) return;
            
            foreach (var endpoint in redis.GetEndPoints())
            {
                var server = redis.GetServer(endpoint);
                    
                server.FlushDatabase(_redisDatabaseIndex);    
            }
        }
        catch
        {
            // ignored
        }
    }
    
    private void ClearDatabaseRecord()
    {
        try
        {
            var connection = new MySqlConnection(new PostBoyConnectionString(CurrentConfiguration).Value);

            var deleteStatements = new List<string>();

            connection.Open();

            using var reader = new MySqlCommand(
                    $"SELECT table_name FROM INFORMATION_SCHEMA.tables WHERE table_schema = '{_databaseName}';",
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
            Console.WriteLine($"Error cleaning up data, {_testTopic}, {ex}");
        }
    }

    public void Dispose()
    {
        ClearDatabaseRecord();
        FlushRedisDatabase();
    }
    
    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }
}