﻿using System.ComponentModel.DataAnnotations;

namespace Un2Trek.Trekis.API.Controllers;

public class RegisterRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public string LastName { get; set; }

    [Required]
    [MinLength(5)]
    public string Password { get; set; }

    public bool ReceivePromotionalEmails { get; set; }
}
