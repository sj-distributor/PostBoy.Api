using System.Reflection;
using Microsoft.EntityFrameworkCore;
using PostBoy.Core.Data;
using PostBoy.Core.Domain;

namespace PostBoy.E2ETests.Mocks;

public class InMemoryDbContext : DbContext, IUnitOfWork
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase(databaseName: "PostBoy");
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