using AuthService.DTOs;
using AuthService.Models;
using Microsoft.AspNetCore.Identity;

namespace AuthService.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtTokenService _jwtTokenService;

        public AuthService(UserManager<ApplicationUser> userManager, IJwtTokenService jwtTokenService)
        {
            _userManager = userManager;
            _jwtTokenService = jwtTokenService;
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

        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if(user == null)
            {
                throw new UnauthorizedAccessException("Invalid email or password.");
            }

            if (!user.IsActive)
            {
                throw new UnauthorizedAccessException("Your account is inactive");
            }

            var passwordValid = await _userManager.CheckPasswordAsync(user, request.Password);

            if (!passwordValid)
            {
                throw new UnauthorizedAccessException("Invalid email or password");
            }

            var (token, expiresAtUtc) = await _jwtTokenService.GenerateTokenAsync(user);

            var roles = await _userManager.GetRolesAsync(user);

            return new LoginResponse
            {
                AccessToken = token,
                TokenType = "Bearer",
                ExpiresAtUtc = expiresAtUtc,
                UserId = user.Id,
                Email = user.Email!,
                Roles = roles
            };
        }
    }
}
