using Microsoft.EntityFrameworkCore;
using NZWalksAPI.Models.Domain;

namespace NZWalksAPI.Data;

public class NZWalksDBContext : DbContext {

    public NZWalksDBContext(DbContextOptions dbContextOptions) : base(dbContextOptions) {

    }

    // DbSets is a collection of entities in the database
    public DbSet<Difficulty> Difficulties { get; set; }
    public DbSet<Region> Regions { get; set; }  
    public DbSet<Walk> Walks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Configure all Guid properties to use uuid
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties())
            {
                if (property.ClrType == typeof(Guid))
                {
                    property.SetColumnType("uuid");
                }
            }
        }
        
        // ... rest of your configurations ...
    }
}
