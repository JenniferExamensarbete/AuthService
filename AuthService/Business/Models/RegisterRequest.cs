using System.ComponentModel.DataAnnotations;

namespace AuthService.Business.Models;

public class RegisterRequest
{
    [Required(ErrorMessage = "Email krävs.")]
    [EmailAddress(ErrorMessage = "Ange en giltig emailadress.")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Lösenord krävs.")]
    [MinLength(6, ErrorMessage = "Lösenordet måste vara minst 6 tecken.")]
    [RegularExpression(
        @"^(?=.*[A-Za-z])(?=.*\d).{6,}$",
        ErrorMessage = "Lösenordet måste innehålla minst en bokstav och en siffra."
    )]
    public string Password { get; set; } = null!;

    [Required(ErrorMessage = "Bekräfta lösenord krävs.")]
    [Compare("Password", ErrorMessage = "Lösenorden matchar inte.")]
    public string ConfirmPassword { get; set; } = null!;

    [RegularExpression(
        @"^[A-Za-zÅÄÖåäö\s-]{2,50}$",
        ErrorMessage = "Förnamn får bara innehålla bokstäver och måste vara 2–50 tecken."
    )]
    public string? FirstName { get; set; }

    [RegularExpression(
        @"^[A-Za-zÅÄÖåäö\s-]{2,50}$",
        ErrorMessage = "Efternamn får bara innehålla bokstäver och måste vara 2–50 tecken."
    )]
    public string? LastName { get; set; }
}