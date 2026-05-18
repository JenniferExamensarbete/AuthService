using AuthService.Business.Interfaces;
using AuthService.Business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController(IAuthService authService) : ControllerBase
{
    private readonly IAuthService _authService = authService;

    [Authorize(Roles = "Admin")]
    [HttpPost("signup")]
    public async Task<IActionResult> Signup(RegisterRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _authService.RegisterAsync(request);

        return result.Success
            ? Ok(result)
            : BadRequest(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _authService.LoginAsync(request);

        return result.Success
            ? Ok(result)
            : Unauthorized(result);
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await _authService.LogoutAsync();
        return Ok();
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> Me()
    {
        var user = await _authService.GetCurrentUserAsync();

        return user == null
            ? Unauthorized()
            : Ok(user);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{authUserId}")]
    public async Task<IActionResult> DeleteUser(string authUserId)
    {
        var result = await _authService.DeleteUserAsync(authUserId);

        return result.Success
            ? Ok(result)
            : NotFound(result);
    }

}