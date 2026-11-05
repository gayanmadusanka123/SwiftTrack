using AuthService.Models;

namespace AuthService.Services;

public interface IJwtTokenService
{
    Task<(string Token, DateTime ExpiresAtUtc)> GenerateTokenAsync(ApplicationUser user);
}
