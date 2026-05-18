using AuthService.Business.Models;

namespace AuthService.Business.Interfaces;

public interface IAuthService
{
    Task<AuthResult> RegisterAsync(RegisterRequest request);
    Task<AuthResult> LoginAsync(LoginRequest request);
    Task LogoutAsync();
    Task<UserDto?> GetCurrentUserAsync();
    Task<AuthResult> DeleteUserAsync(string authUserId);
}