using System.ComponentModel.DataAnnotations;

namespace Appointement.ViewModels
{
    public class UserRegisterViewModel
    {
        [EmailAddress(ErrorMessage = "Invalid Email")]
        public string Email { get; set; }
        [Phone(ErrorMessage ="Invalid Phone Number")]
        public string PhoneNumber { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage ="Password and Confirm Password don't match")]
        public string ConfirmPassword { get; set; }
    }
}
