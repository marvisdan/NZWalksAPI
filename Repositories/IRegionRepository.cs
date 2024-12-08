using NZWalksAPI.Models.Domain;
public interface IRegionRepository
{
    Task<List<Region>> GetAllAsync();
    Task<Region?> GetByIdAsync(string id);
    Task<Region> CreateAsync(Region region);
    Task<Region?> UpdateAsync(string id, Region region);
    Task<Region?> DeleteAsync(string id);
}
