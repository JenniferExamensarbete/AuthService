namespace AuthService.Business.Models;

public class AuthResult
{
    public bool Success { get; set; }
    public string? Error { get; set; }
    public UserDto? User { get; set; }
}