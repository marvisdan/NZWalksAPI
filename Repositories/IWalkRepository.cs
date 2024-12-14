using NZWalksAPI.Models.Domain;

public interface IWalkRepository
{
    Task<Walk> CreateAsync(Walk walk);
    Task<List<Walk>> GetAllAsync(string? filterOn = null, string? filterQuery = null);
    Task<Walk> GetByIdAsync(string id);
    Task<Walk?> UpdateAsync(string id, Walk walk);
    Task<Walk?> DeleteAsync(string id);
}