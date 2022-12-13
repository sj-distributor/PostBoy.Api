using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using MySql.Data.MySqlClient;
using PostBoy.Core.Services.Identity;
using PostBoy.Core.Settings.System;
using PostBoy.Api;

namespace PostBoy.E2ETests;

public class ApiTestFixture : WebApplicationFactory<Startup>
{
    private readonly List<string> _tableRecordsDeletionExcludeList = new()
    {
        "schemaversions"
    };
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureTestServices(services =>
        {
            services.AddTransient<ICurrentUser, TestCurrentUser>();
        });
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
}

internal class TestCurrentUser : ICurrentUser
{
    public Guid Id => Guid.Empty;
}