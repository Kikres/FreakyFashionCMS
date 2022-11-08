﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace FreakyFashion.Models.ViewModels.Forms;

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