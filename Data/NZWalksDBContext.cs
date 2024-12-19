using Microsoft.EntityFrameworkCore;
using NZWalksAPI.Models.Domain;

namespace NZWalksAPI.Data;

public class NZWalksDBContext : DbContext
{

    public NZWalksDBContext(DbContextOptions<NZWalksDBContext> dbContextOptions) : base(dbContextOptions)
    {

    }

    // DbSets is a collection of entities in the database
    public DbSet<Difficulty> Difficulties { get; set; }
    public DbSet<Region> Regions { get; set; }
    public DbSet<Walk> Walks { get; set; }
    public DbSet<Image> Images { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // Seed the data for difficulties
        //Easy, Hard, Medium
        var diffuculties = new List<Difficulty>() {
            new Difficulty() {
                Id = 1 ,
                Name = "Easy"
            },
            new Difficulty() {
                Id = 2,
                Name = "Medium"
            },
            new Difficulty() {
                Id = 3,
                Name = "Hard"
            }
        };

        // Seed difficulties into the database
        modelBuilder.Entity<Difficulty>().HasData(diffuculties);


        // Seed data for Regions

        var regions = new List<Region>()
        {
            new Region() {
                Id = "f7248fc3-2585-4efb-8d1d-1c555f4087f5",
                Name = "Auckland",
                Code = "AKL",
                RegionImageUrl = "https://images.pexels.com/photos/5169939/pexels-photo-5169939.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=2"
            },
            new Region() {
                Id = "688429e8-ad80-4585-b7f4-4c39ee1d8f54",
                Name = "Wellington",
                Code = "WLG",
                RegionImageUrl = "https://images.pexels.com/photos/5169939/pexels-photo-5169939.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=2"
            },
            new Region() {
                Id = "14ceba71-4b51-4777-9b17-a5156b042262",
                Name = "Nelson",
                Code = "NSN",
                RegionImageUrl = "https://images.pexels.com/photos/5169939/pexels-photo-5169939.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=2"
            },
            new Region() {
                Id = "cfa06ed2-bf65-4b65-948d-d5e7848ab5a1",
                Name = "Southland",
                Code = "STL",
                RegionImageUrl = "https://images.pexels.com/photos/5169939/pexels-photo-5169939.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=2"
            },
            new Region() {
                Id = "688429e8-ad80-4585-b7f4-4c39ee1d8f57",
                Name = "Wellington",
                Code = "WLG",
                RegionImageUrl = "https://images.pexels.com/photos/5169939/pexels-photo-5169939.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=2"
            }
        };


        modelBuilder.Entity<Region>().HasData(regions);

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

        // Configure the Id property to use uuid
        modelBuilder.Entity<Region>()
            .Property(x => x.Id)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<Walk>()
            .Property(x => x.Id)
            .ValueGeneratedOnAdd();

    }
}
