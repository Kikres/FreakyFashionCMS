using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FreakyFashion.Models.ViewModels;

public class RegisterViewModel
{
    [DisplayName("Förnamn")]
    [Required(ErrorMessage = "Fältet Förnamn måste fyllas i")]
    public string FirstName { get; set; }

    [DisplayName("Efternamn")]
    [Required(ErrorMessage = "Fältet Efternamn måste fyllas i")]
    public string LastName { get; set; }

    [DisplayName("E-post")]
    [Required(ErrorMessage = "Fältet E-post måste fyllas i")]
    public string Email { get; set; }

    [DisplayName("Lösenord")]
    [Required(ErrorMessage = "Fältet Lösenord måste fyllas i")]
    public string Password { get; set; }
}