
namespace NZWalksAPI.Models.Domain;

public class Walk
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public double LengthInKm { get; set; }
    public string? WalkImageUrl { get; set; }
    public int DifficultyId { get; set; }
    public string RegionId { get; set; }

    // Navigation Properties
    public Difficulty? Difficulty { get; set; }
    public Region? Region { get; set; }
}