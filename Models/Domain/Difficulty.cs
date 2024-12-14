using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NZWalksAPI.Models.Domain;
public class Difficulty
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public required string Name { get; set; } // Difficulty name
}