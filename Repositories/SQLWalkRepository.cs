using NZWalksAPI.Data;
using NZWalksAPI.Models.Domain;
using Microsoft.EntityFrameworkCore;

public class SQLWalkRepository : IWalkRepository
{
	private readonly NZWalksDBContext dbContext;

	public SQLWalkRepository(NZWalksDBContext dbContext)
	{
		this.dbContext = dbContext;
	}
	public async Task<Walk> CreateAsync(Walk walk)
	{
		await dbContext.Walks.AddAsync(walk);
		await dbContext.SaveChangesAsync();
		return walk;

	}
	public async Task<List<Walk>> GetAllAsync(string? filterOn = null, string? filterQuery = null, string? sortBy = null, bool isAscending = true, int pageNumber = 1, int pageSize = 1000)
	{
		var walks = dbContext.Walks.Include("Region").Include("Difficulty").AsQueryable();

		//Filtering
		if (string.IsNullOrWhiteSpace(filterOn) == false && string.IsNullOrWhiteSpace(filterQuery) == false)
		{
			if (filterOn.Equals("Name", StringComparison.OrdinalIgnoreCase))
			{
				walks = walks.Where(x => x.Name.Contains(filterQuery));
			}
		}
		// Sorting
		if (string.IsNullOrWhiteSpace(sortBy) == false)
		{
			if (sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
			{
				walks = isAscending ? walks.OrderBy(x => x.Name) : walks.OrderByDescending(x => x.Name);
			}
			else if (sortBy.Equals("Length", StringComparison.OrdinalIgnoreCase))
			{
				walks = isAscending ? walks.OrderBy(x => x.LengthInKm) : walks.OrderByDescending(x => x.LengthInKm);
			}
		}
		// Pagination
		var skipResults = (pageNumber - 1) * pageSize;
		return await walks.Skip(skipResults).Take(pageSize).ToListAsync();
	}

	public async Task<Walk> GetByIdAsync(string id)
	{
		return await dbContext.Walks
			.Include("Region")
			.Include("Difficulty")
			.FirstOrDefaultAsync(x => x.Id == id);
	}
	public async Task<Walk?> UpdateAsync(string id, Walk walk)
	{
		var existingWalk = await dbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);
		if (existingWalk == null)
		{
			return null;
		}

		existingWalk.Name = walk.Name;
		existingWalk.Description = walk.Description;
		existingWalk.LengthInKm = walk.LengthInKm;
		existingWalk.WalkImageUrl = walk.WalkImageUrl;
		existingWalk.DifficultyId = walk.DifficultyId;
		existingWalk.RegionId = walk.RegionId;

		await dbContext.SaveChangesAsync();
		return existingWalk;
	}
	public async Task<Walk?> DeleteAsync(string id)
	{
		var existingWalk = await dbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);
		if (existingWalk == null)
		{
			return null;
		}
		dbContext.Walks.Remove(existingWalk);
		await dbContext.SaveChangesAsync();
		return existingWalk;
	}
}