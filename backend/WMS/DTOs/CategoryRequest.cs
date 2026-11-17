using System.ComponentModel.DataAnnotations;

namespace WMS.DTOs
{
    public class CategoryRequest
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }
    }
}
