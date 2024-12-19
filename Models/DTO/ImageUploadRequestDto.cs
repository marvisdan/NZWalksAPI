using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace NZWalksAPI.Models.DTO
{
    public class ImageUploadRequestDto
    {
        public IFormFile File { get; set; }
        [Required]
        public string FileName { get; set; }
        public string? FileDescription { get; set; }
        public string? FileExtension { get; set; }
    }
}