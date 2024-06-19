using System.ComponentModel.DataAnnotations;

namespace Appointement.ViewModels
{
    public class UpdateUserEmailViewModel
    {
        public string UserName { get; set; }
        [EmailAddress(ErrorMessage ="you enterd an invalid email.")]
        public string Email { get; set; }
        [Phone(ErrorMessage = "you enterd an invalid phone number.")]
        public string PhoneNumber { get; set; }
    }
}
