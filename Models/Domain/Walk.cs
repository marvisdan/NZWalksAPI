
namespace NZWalksAPI.Models.Domain;

public class Walk {
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required double LengthInKm { get; set; }
    public string? WalkImageUrl { get; set; }
    public int DifficultyId { get; set; }
    public int RegionId { get; set; }

    // Navigation Property
    public Difficulty? Difficulty { get; set; }
    public Region? Region { get; set; }
}