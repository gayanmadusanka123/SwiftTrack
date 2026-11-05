using AuthService.DTOs;
using AuthService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers
{
    [ApiController]
    [Route("api/role-management")]
    [Authorize(Roles = "Admin")]
    public class RoleManagementController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleManagementController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpPost("{userId}/assign")]
        public async Task<IActionResult> AssignRole(
            string userId, 
            [FromBody] AssignRoleRequest request)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if(user == null)
            {
                return NotFound(new
                {
                    Message = "User not Found"
                });
            }

            var role = request.Role.Trim().ToLowerInvariant() switch
            {
                "client" => "Client",
                "driver" => "Driver",
                "admin" => "Admin",
                _ => null
            };

            if(role == null)
            {
                return BadRequest(new
                {
                    Message = "Invalid Role"
                });
            }

            if(!await _roleManager.RoleExistsAsync(role))
            {
                return BadRequest(new
                {
                    Message = "The requested role does not exist"
                });
            }

            //prevent duplicates
            if(await _userManager.IsInRoleAsync(user, role))
            {
                return Conflict(new
                {
                    Message = $"User already has the {role} role."
                });
            }

            var result = await _userManager.AddToRoleAsync(user, role);

            if (!result.Succeeded)
            {
                return BadRequest(new
                {
                    Message = "Role assignment failed",
                    Errors = result.Errors.Select(error => error.Description)
                });
            }

            return Ok(new
            {
                Message = $"Successfully assigned the {role} role.",
                UserId = user.Id,
                Email = user.Email,
                Role = role
            });
        }
    }
}
