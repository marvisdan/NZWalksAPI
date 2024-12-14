using NZWalksAPI.Data;
using System.ComponentModel.DataAnnotations;
public class AddWalkRequestDto
{
    [Required]
    [MaxLength(100, ErrorMessage = "Name has to be a maximum of 100 characters")]
    public string Name { get; set; }
    [Required]
    [MaxLength(1000, ErrorMessage = "Description has to be a maximum of 1000 characters")]
    public string Description { get; set; }
    [Required]
    [Range(0, 50, ErrorMessage = "Length must be between 0 and 50")]
    public double LengthInKm { get; set; }
    public string? WalkImageUrl { get; set; }
    [Required]
    public int DifficultyId { get; set; }
    [Required]
    public string RegionId { get; set; }
}