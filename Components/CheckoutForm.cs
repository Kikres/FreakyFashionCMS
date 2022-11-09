using FreakyFashion.Services;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace FreakyFashion.Components;

public class CheckoutForm : ViewComponent
{
    private readonly ICustomMemberService _customMemberService;

    public CheckoutForm(ICustomMemberService customMemberService)
    {
        _customMemberService = customMemberService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        if (_customMemberService.IsLoggedIn())
        {
            var user = _customMemberService.GetLoggedInMember();
            var name = user.Name.Split(" ");
            var viewModel = new CheckoutExistingFormViewModel { FirstName = name[0], LastName = name[1], Email = user.Email };
            return View("CheckoutExistingForm", viewModel);
        }

        return View("CheckoutNewForm", new CheckoutNewFormViewModel());
    }
}

public class CheckoutExistingFormViewModel
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
}

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