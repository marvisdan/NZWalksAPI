using NZWalksAPI.Models.DTO;

public class WalkDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public double LengthInKm { get; set; }
    public string? WalkImageUrl { get; set; }
    public RegionDto Region { get; set; }
    public DifficultyDto Difficulty { get; set; }
}