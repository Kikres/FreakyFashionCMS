using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace FreakyFashion.Models.ViewModels.Forms;

public class CheckoutNewFormViewModel
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
    [MinLength(8, ErrorMessage = "Lösenorder måste var minst 8 tecken")]
    public string? Password { get; set; }
}