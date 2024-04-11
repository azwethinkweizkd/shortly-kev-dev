﻿using Shortly.Client.Helpers.Validators;
using System.ComponentModel.DataAnnotations;

namespace Shortly.Client.Data.ViewModels
{
    public class RegisterVm
    {
        [Required(ErrorMessage = "Full name is required")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Email address is required")]
        [CustomEmailValidator(ErrorMessage = "Email address is not valid")]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MinLength(5, ErrorMessage = "Password must be at least 5 characters")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm password is required")]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }
    }
}