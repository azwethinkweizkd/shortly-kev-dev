namespace Shortly.Client.Data.ViewModels
{
    public class ConfirmEmailLoginVm
    {
        [Required(ErrorMessage = "Email address is required")]
        [CustomEmailValidator(ErrorMessage = "Email address is not valid")]
        public string EmailAddress { get; set; }

    }
}
