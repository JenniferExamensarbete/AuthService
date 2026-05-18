using System.ComponentModel.DataAnnotations;

namespace AuthService.Business.Models;

public class LoginRequest
{
    [Required(ErrorMessage = "Email krävs.")]
    [EmailAddress(ErrorMessage = "Ange en giltig emailadress.")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Lösenord krävs.")]
    public string Password { get; set; } = null!;
}