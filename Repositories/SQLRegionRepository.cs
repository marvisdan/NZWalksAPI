using Microsoft.EntityFrameworkCore;
using NZWalksAPI.Data;
using NZWalksAPI.Models.Domain;

public class SQLRegionRepository : IRegionRepository
{
    private readonly NZWalksDBContext dbContext;

    public SQLRegionRepository(NZWalksDBContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<List<Region>> GetAllAsync()
    {
        return await dbContext.Regions.ToListAsync();
    }

    public async Task<Region?> GetByIdAsync(string id)
    {
        return await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Region> CreateAsync(Region region)
    {
        await dbContext.Regions.AddAsync(region);
        await dbContext.SaveChangesAsync();
        return region;
    }

    public async Task<Region?> UpdateAsync(string id, Region region)
    {
        var existingRegion = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
        if (existingRegion == null)
        {
            return null;
        }

        existingRegion.Name = region.Name;
        existingRegion.Code = region.Code;
        existingRegion.RegionImageUrl = region.RegionImageUrl;
        await dbContext.SaveChangesAsync();
        return existingRegion;
    }

    public async Task<Region?> DeleteAsync(string id)
    {
        var existingRegion = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
        if (existingRegion == null)
        {
            return null;
        }

        dbContext.Regions.Remove(existingRegion);
        await dbContext.SaveChangesAsync();
        return existingRegion;
    }
}
