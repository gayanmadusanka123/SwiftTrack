using AuthService.DTOs;
using AuthService.Models;
using Microsoft.AspNetCore.Identity;

namespace AuthService.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
        {
            var existingUser = await _userManager.FindByEmailAsync(request.Email);

            if (existingUser != null)
            {
                throw new InvalidOperationException("Email is already registered.");
            }

            var user = new ApplicationUser
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.Email,
                Email = request.Email,
                NIC = request.NIC,
                Address = request.Address
            };

            // user creation and password hashing
            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new InvalidOperationException(errors);
            }

            var roleResult = await _userManager.AddToRoleAsync(user, "Client");

            if (!roleResult.Succeeded)
            {
                await _userManager.DeleteAsync(user);
                throw new InvalidOperationException("Could not assign Client role.");
            }

            return new AuthResponse
            {
                Message = "Registration successfull.",
                UserId = user.Id,
                Email = user.Email!
            };

        }
    }
}
