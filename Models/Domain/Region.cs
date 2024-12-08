namespace NZWalksAPI.Models.Domain;

public class Region
{
    public string? Id { get; set; }
    public string? Code { get; set; }
    public required string Name { get; set; }
    public string? RegionImageUrl { get; set; }
}
