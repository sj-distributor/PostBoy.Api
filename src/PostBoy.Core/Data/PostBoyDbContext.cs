using System.Reflection;
using Microsoft.EntityFrameworkCore;
using PostBoy.Core.Domain;
using PostBoy.Core.Settings.System;

namespace PostBoy.Core.Data;

public class PostBoyDbContext : DbContext, IUnitOfWork
{
    private readonly PostBoyConnectionString _connectionString;

    public PostBoyDbContext(PostBoyConnectionString connectionString)
    {
        _connectionString = connectionString;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMySql(_connectionString.Value, new MySqlServerVersion(new Version(8, 0, 28)));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        typeof(PostBoyDbContext).GetTypeInfo().Assembly.GetTypes()
            .Where(t => typeof(IEntity).IsAssignableFrom(t) && t.IsClass).ToList()
            .ForEach(x =>
            {
                if (modelBuilder.Model.FindEntityType(x) == null)
                    modelBuilder.Model.AddEntityType(x);
            });
    }
    
    public bool ShouldSaveChanges { get; set; }
}