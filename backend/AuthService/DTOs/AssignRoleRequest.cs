using System.ComponentModel.DataAnnotations;

namespace AuthService.DTOs
{
    public class AssignRoleRequest
    {
        [Required]
        public string Role { get; set; } = string.Empty;
    }
}
