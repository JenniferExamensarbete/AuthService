using System.ComponentModel.DataAnnotations;

namespace AuthService.Business.Models;

public class RegisterRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Required]
    public string Password { get; set; } = null!;

    [Required]
    public string ConfirmPassword { get; set; } = null!;

    public string? FirstName { get; set; }

    public string? LastName { get; set; }
}