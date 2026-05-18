using AuthService.Business.Interfaces;
using AuthService.Business.Models;
using AuthService.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace AuthService.Business.Services;

public class AuthService(
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager,
    IHttpContextAccessor httpContextAccessor) : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public async Task<AuthResult> RegisterAsync(RegisterRequest request)
    {
        if (request.Password != request.ConfirmPassword)
        {
            return new AuthResult
            {
                Success = false,
                Error = "Passwords do not match."
            };
        }

        var user = new ApplicationUser
        {
            UserName = request.Email,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName
        };

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            return new AuthResult
            {
                Success = false,
                Error = string.Join(", ", result.Errors.Select(x => x.Description))
            };
        }

        await _userManager.AddToRoleAsync(user, "Employee");

        return new AuthResult
        {
            Success = true,
            User = await CreateUserDtoAsync(user)
        };
    }

    public async Task<AuthResult> LoginAsync(LoginRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user == null)
        {
            return new AuthResult
            {
                Success = false,
                Error = "Invalid email or password."
            };
        }

        var result = await _signInManager.PasswordSignInAsync(
            user,
            request.Password,
            isPersistent: true,
            lockoutOnFailure: false
        );

        if (!result.Succeeded)
        {
            return new AuthResult
            {
                Success = false,
                Error = "Invalid email or password."
            };
        }

        return new AuthResult
        {
            Success = true,
            User = await CreateUserDtoAsync(user)
        };
    }

    public async Task LogoutAsync()
    {
        await _signInManager.SignOutAsync();
    }

    public async Task<UserDto?> GetCurrentUserAsync()
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId == null)
            return null;

        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
            return null;

        return await CreateUserDtoAsync(user);
    }

    public async Task<AuthResult> DeleteUserAsync(string authUserId)
    {
        var user = await _userManager.FindByIdAsync(authUserId);

        if (user == null)
        {
            return new AuthResult
            {
                Success = false,
                Error = "User not found."
            };
        }

        var currentUserId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (currentUserId == authUserId)
        {
            return new AuthResult
            {
                Success = false,
                Error = "You cannot delete your own admin account."
            };
        }

        var result = await _userManager.DeleteAsync(user);

        if (!result.Succeeded)
        {
            return new AuthResult
            {
                Success = false,
                Error = string.Join(", ", result.Errors.Select(x => x.Description))
            };
        }

        return new AuthResult
        {
            Success = true
        };
    }

    private async Task<UserDto> CreateUserDtoAsync(ApplicationUser user)
    {
        var roles = await _userManager.GetRolesAsync(user);

        return new UserDto
        {
            Id = user.Id,
            Email = user.Email!,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Role = roles.FirstOrDefault() ?? "Employee"
        };
    }
}