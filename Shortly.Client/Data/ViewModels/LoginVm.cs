using System.ComponentModel.DataAnnotations;

namespace Shortly.Client.Data.ViewModels
{
    public class LoginVm
    {
        [Required(ErrorMessage = "Email address is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [RegularExpression(@"^\S+@\S+\.\S+$", ErrorMessage = "Invalid email address")]
        public string EmailAddress { get; set; }


        [Required(ErrorMessage = "Password is required")]
        [MinLength(5, ErrorMessage = "Password must be at least 5 characters")]
        [MaxLength(16, ErrorMessage = "Password must be at less than 16 characters")]
        public string Password { get; set; }
    }
}
