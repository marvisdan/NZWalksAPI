using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
namespace NZWalksAPI.Data;

public class NZWalksAuthDBContext : IdentityDbContext
{
    public NZWalksAuthDBContext(DbContextOptions<NZWalksAuthDBContext> options) : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        var readerRoleId = "8f7768c4-72b9-4c73-b52d-f5c5c1e0f4e7";
        var writerRoleId = "8f7768c4-72b9-4c73-b52d-f5c5c1e0f4e8";
        var roles = new List<IdentityRole> {
            new IdentityRole() {
                Id = readerRoleId,
                Name = "Reader",
                NormalizedName = "Reader".ToUpper()
            },
            new IdentityRole() {
                Id = writerRoleId,
                ConcurrencyStamp = writerRoleId,
                Name = "Writer",
                NormalizedName = "Writer".ToUpper()
            }
        };
        // Seed the data for roles
        builder.Entity<IdentityRole>().HasData(roles);
    }
}