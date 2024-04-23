using Microsoft.AspNetCore.Authentication;
using Shortly.Client.Helpers.Validators;
using System.ComponentModel.DataAnnotations;

namespace Shortly.Client.Data.ViewModels
{
    public class LoginVm
    {
        [Required(ErrorMessage = "Email address is required")]
        [CustomEmailValidator(ErrorMessage = "Email address is not valid")]
        public string EmailAddress { get; set; }


        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        public IEnumerable<AuthenticationScheme> Schemes { get; set; }
    }
}
